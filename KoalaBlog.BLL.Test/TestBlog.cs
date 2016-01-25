using KoalaBlog.BLL.Handlers;
using KoalaBlog.DAL;
using KoalaBlog.Entity.Models;
using KoalaBlog.Entity.Models.Enums;
using NUnit.Framework;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoalaBlog.Framework.Exceptions;
using System.Diagnostics;

namespace KoalaBlog.BLL.Test
{
    [TestFixture]
    class TestBlog
    {
        private Person testPerson;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            TestUtil.CleanUpData();

            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                PersonHandler perHandler = new PersonHandler(dbContext);
                testPerson = new Person();
                testPerson.RealName = "testRealName";
                testPerson.NickName = "testPerson";
                testPerson.RealNameAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                testPerson.SexualTrendAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                testPerson.MaritalStatusAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                testPerson.QQAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                testPerson.DOBAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                testPerson.BloodTypeAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                testPerson.HomePageAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                perHandler.Add(testPerson);
                perHandler.SaveChanges();
            }
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            TestUtil.CleanUpData();
        }

        [Test]
        public async Task Test_01_CreateBlogWithoutContentAsync()
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);
                GroupHandler groupHandler = new GroupHandler(dbContext);
                BlogAccessControlHandler acHandler = new BlogAccessControlHandler(dbContext);
                BlogAccessControlXGroupHandler acxgHandler = new BlogAccessControlXGroupHandler(dbContext);

                //1. test normal.
                Blog newBlog = await blogHandler.CreateBlogAsync(testPerson.ID, "testBlog", BlogInfoAccessInfo.All, null);

                Blog testBlog_1 = await blogHandler.GetByIdAsync(newBlog.ID);
                BlogAccessControl testAC_1 = await acHandler.Fetch(x => x.BlogID == testBlog_1.ID).FirstOrDefaultAsync();
                
                Assert.IsNotNull(testBlog_1);
                Assert.IsNotNull(testAC_1);

                Assert.AreEqual(testBlog_1.PersonID, testPerson.ID);
                Assert.AreEqual(testBlog_1.Content, "testBlog");
                Assert.AreEqual(testAC_1.AccessLevel, BlogInfoAccessInfo.All);

                //2. set BlogInfoAccessInfo GroupOnly and set GroupID is null then check it.
                bool isChecked = false;
                try
                {
                    newBlog = await blogHandler.CreateBlogAsync(testPerson.ID, "testBlogTwo", BlogInfoAccessInfo.GroupOnly, null);
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(DisplayableException));
                    Assert.AreEqual(ex.Message, "未指定组");
                }
                Assert.IsTrue(isChecked);

                //3. set not exist GroupID. expect Exception is type of foreign key exception.
                isChecked = false;
                try
                {
                    newBlog = await blogHandler.CreateBlogAsync(testPerson.ID, "testBlogThree", BlogInfoAccessInfo.GroupOnly, 9999);
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreNotEqual(ex.GetType(), typeof(AssertException));
                }
                Assert.IsTrue(isChecked);
            }

            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);
                GroupHandler groupHandler = new GroupHandler(dbContext);
                BlogAccessControlHandler acHandler = new BlogAccessControlHandler(dbContext);
                BlogAccessControlXGroupHandler acxgHandler = new BlogAccessControlXGroupHandler(dbContext);

                //4. create test group object and test it.
                Group testGroup = await groupHandler.CreateGroupAsync(testPerson.ID, "testGroup", GroupType.GroupList);
                bool isChecked = false;
                try
                {
                    Blog testBlog4Group = await blogHandler.CreateBlogAsync(testPerson.ID, "testBlogFour", BlogInfoAccessInfo.GroupOnly, testGroup.ID);

                    Blog testBlog_2 = await blogHandler.GetByIdAsync(testBlog4Group.ID);
                    BlogAccessControl testAC_2 = await acHandler.Fetch(x => x.BlogID == testBlog_2.ID).FirstOrDefaultAsync();
                    BlogAccessControlXGroup testACXG = await acxgHandler.Fetch(x => x.GroupID == testGroup.ID && x.BlogAccessControlID == testAC_2.ID).FirstOrDefaultAsync();

                    Assert.IsNotNull(testBlog_2);
                    Assert.IsNotNull(testAC_2);
                    Assert.IsNotNull(testACXG);

                    Assert.AreEqual(testBlog_2.PersonID, testPerson.ID);
                    Assert.AreEqual(testBlog_2.Content, "testBlogFour");
                    Assert.AreEqual(testAC_2.AccessLevel, BlogInfoAccessInfo.GroupOnly);
                }
                catch (Exception)
                {
                    isChecked = true;
                }
                Assert.IsFalse(isChecked);
            }
        }

        [Test]
        public async Task Test_02_CreateBlogIncludeContentAsync()
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);
                ContentHandler contentHandler = new ContentHandler(dbContext);
                BlogXContentHandler bxcHandler = new BlogXContentHandler(dbContext);
                GroupHandler groupHandler = new GroupHandler(dbContext);
                BlogAccessControlHandler acHandler = new BlogAccessControlHandler(dbContext);
                BlogAccessControlXGroupHandler acxgHandler = new BlogAccessControlXGroupHandler(dbContext);

                List<long> contentIds = new List<long>();

                Content content = new Content()
                {
                    ContentPath = "testPath",
                    ContentBinary = new byte[] { 1, 3, 5 },
                    Type = ContentType.Photo,
                    MimeType = "jpg"
                };
                contentHandler.Add(content);
                contentHandler.SaveChanges();

                contentIds.Add(content.ID);

                Stopwatch sw = Stopwatch.StartNew();

                //1. test normal.
                Blog testBlog = await blogHandler.CreateBlogAsync(testPerson.ID, "testBlog", BlogInfoAccessInfo.All, null, contentIds);

                var time = sw.ElapsedMilliseconds;

                Blog testBlog_1 = await blogHandler.GetByIdAsync(testBlog.ID);
                BlogAccessControl testAC_1 = await acHandler.Fetch(x => x.BlogID == testBlog_1.ID).FirstOrDefaultAsync();
                BlogXContent testBXC_1 = await bxcHandler.Fetch(x => x.BlogID == testBlog_1.ID).FirstOrDefaultAsync();
                Content testContent_1 = testBXC_1.Content;

                Assert.IsNotNull(testBlog_1);
                Assert.IsNotNull(testAC_1);
                Assert.IsNotNull(testBXC_1);
                Assert.IsNotNull(testContent_1);

                Assert.AreEqual(testBlog_1.PersonID, testPerson.ID);
                Assert.AreEqual(testBlog_1.Content, "testBlog");
                Assert.AreEqual(testAC_1.AccessLevel, BlogInfoAccessInfo.All);
                Assert.AreEqual(testContent_1.ContentPath, "testPath");
                Assert.AreEqual(testContent_1.ContentBinary, new byte[] { 1, 3, 5 });

                //2. create test group object and test it.
                contentIds = new List<long>();

                Content content1 = new Content()
                {                    
                    ContentPath = "testFilePathYesOrNo",
                    ContentBinary = new byte[] { 23, 31, 45, 78, 99 },
                    Type = ContentType.Video,
                    MimeType = "jpg"
                };
                contentHandler.Add(content1);
                contentHandler.SaveChanges();

                contentIds.Add(content1.ID);

                Group testGroup = await groupHandler.CreateGroupAsync(testPerson.ID, "testGroup", GroupType.GroupList);

                testBlog = await blogHandler.CreateBlogAsync(testPerson.ID, "testBlogGroupOnly", BlogInfoAccessInfo.GroupOnly, testGroup.ID, contentIds);

                Blog testBlog_2 = await blogHandler.GetByIdAsync(testBlog.ID);
                BlogAccessControl testAC_2 = await acHandler.Fetch(x => x.BlogID == testBlog_2.ID).FirstOrDefaultAsync();
                BlogAccessControlXGroup testACXG_2 = await acxgHandler.Fetch(x => x.GroupID == testGroup.ID && x.BlogAccessControlID == testAC_2.ID).FirstOrDefaultAsync();
                BlogXContent testBXC_2 = await bxcHandler.Fetch(x => x.BlogID == testBlog_2.ID).FirstOrDefaultAsync();
                Content testContent_2 = testBXC_2.Content;

                Assert.IsNotNull(testBlog_2);
                Assert.IsNotNull(testAC_2);
                Assert.IsNotNull(testACXG_2);
                Assert.IsNotNull(testBXC_2);
                Assert.IsNotNull(testContent_2);

                Assert.AreEqual(testBlog_2.PersonID, testPerson.ID);
                Assert.AreEqual(testBlog_2.Content, "testBlogGroupOnly");
                Assert.AreEqual(testAC_2.AccessLevel, BlogInfoAccessInfo.GroupOnly);
                Assert.AreEqual(testContent_2.ContentPath, "testFilePathYesOrNo");
                Assert.AreEqual(testContent_2.ContentBinary, new byte[] { 23, 31, 45, 78, 99 });
                Assert.AreEqual(testContent_2.Type, ContentType.Video);    
            }

            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);
                ContentHandler contentHandler = new ContentHandler(dbContext);
                BlogXContentHandler bxcHandler = new BlogXContentHandler(dbContext);
                GroupHandler groupHandler = new GroupHandler(dbContext);
                BlogAccessControlHandler acHandler = new BlogAccessControlHandler(dbContext);
                BlogAccessControlXGroupHandler acxgHandler = new BlogAccessControlXGroupHandler(dbContext);

                List<long> contentIds = new List<long>();

                //3. create test group object but set BlogInfoAccessInfo other value.

                Content content1 = new Content()
                {
                    ContentPath = "testFilePathYesOrNo",
                    ContentBinary = new byte[] { 23, 31, 45, 78, 99 },
                    Type = ContentType.Video,
                    MimeType = "jpg"
                };
                contentHandler.Add(content1);
                contentHandler.SaveChanges();

                contentIds.Add(content1.ID);

                Group testGroup_1 = await groupHandler.CreateGroupAsync(testPerson.ID, "testGroupWithTest", GroupType.GroupList);

                Blog testBlog = await blogHandler.CreateBlogAsync(testPerson.ID, "testBlogNotSetGroupOnly", BlogInfoAccessInfo.MyselfOnly, testGroup_1.ID, contentIds);

                Blog testBlog_3 = await blogHandler.GetByIdAsync(testBlog.ID);
                BlogAccessControl testAC_3 = await acHandler.Fetch(x => x.BlogID == testBlog_3.ID).FirstOrDefaultAsync();
                BlogAccessControlXGroup testACXG_3 = await acxgHandler.Fetch(x => x.GroupID == testGroup_1.ID && x.BlogAccessControlID == testAC_3.ID).FirstOrDefaultAsync();
                BlogXContent testBXC_3 = await bxcHandler.Fetch(x => x.BlogID == testBlog_3.ID).FirstOrDefaultAsync();
                Content testContent_3 = testBXC_3.Content;

                Assert.IsNotNull(testBlog_3);
                Assert.IsNotNull(testAC_3);
                Assert.IsNotNull(testBXC_3);
                Assert.IsNotNull(testContent_3);

                //it expect null because BlogInfoAccessInfo is not GroupOnly.
                Assert.IsNull(testACXG_3);

                Assert.AreEqual(testBlog_3.PersonID, testPerson.ID);
                Assert.AreEqual(testBlog_3.Content, "testBlogNotSetGroupOnly");
                Assert.AreEqual(testAC_3.AccessLevel, BlogInfoAccessInfo.MyselfOnly);
                Assert.AreEqual(testContent_3.ContentPath, "testFilePathYesOrNo");
                Assert.AreEqual(testContent_3.ContentBinary, new byte[] { 23, 31, 45, 78, 99 });
                Assert.AreEqual(testContent_3.Type, ContentType.Video);

                //4. create multiple content and test it.
                contentIds = new List<long>();

                for (int i = 0; i < 3; i++)
                {
                    Content c = new Content()
                    {
                        ContentPath = "testMultiplePath",
                        ContentBinary = new byte[] { 12, 3, 4, 5, 7 },
                        Type = ContentType.Video,
                        MimeType = "jpg"
                    };
                    contentHandler.Add(c);
                    contentHandler.SaveChanges();

                    contentIds.Add(c.ID);
                }
                
                bool isChecked = false;
                try
                {
                    Blog testBlog_MultipleContent_1 = await blogHandler.CreateBlogAsync(testPerson.ID, "testBlogMultipleContent1", BlogInfoAccessInfo.MyselfOnly, testGroup_1.ID, contentIds);
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(DisplayableException));
                    Assert.AreEqual(ex.Message, "发表视频不能超过1个");
                }
                Assert.IsTrue(isChecked);

                //5. create multiple content but different type and test it.
                contentIds = new List<long>();

                Content cPhoto = new Content()
                {
                    ContentPath = "testMultiplePath",
                    ContentBinary = new byte[] { 12, 3, 4, 5, 7 },
                    Type = ContentType.Photo,
                    MimeType = "jpg"
                };
                Content cMusic = new Content()
                {
                    ContentPath = "testMultiplePath",
                    ContentBinary = new byte[] { 12, 3, 4, 5, 7 },
                    Type = ContentType.Photo,
                    MimeType = "jpg"
                };
                Content cVideo = new Content()
                {
                    ContentPath = "testMultiplePath",
                    ContentBinary = new byte[] { 12, 3, 4, 5, 7 },
                    Type = ContentType.Video,
                    MimeType = "jpg"
                };
                contentHandler.Add(cPhoto);
                contentHandler.Add(cMusic);
                contentHandler.Add(cVideo);
                contentHandler.SaveChanges();

                contentIds.Add(cPhoto.ID);
                contentIds.Add(cMusic.ID);
                contentIds.Add(cVideo.ID);

                isChecked = false;
                try
                {
                    Blog testBlog_MultipleContent_2 = await blogHandler.CreateBlogAsync(testPerson.ID, "testBlogMultipleContent1", BlogInfoAccessInfo.MyselfOnly, testGroup_1.ID, contentIds);
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(DisplayableException));
                    Assert.AreEqual(ex.Message, "不能发不同类型内容的Blog");
                }
                Assert.IsTrue(isChecked);

                //6. create some photo content and test it.
                contentIds = new List<long>();

                for (int i = 0; i < 6; i++)
                {
                    Content photo = new Content()
                    {
                        ContentPath = "testPhotoContentPath",
                        ContentBinary = new byte[] { 12, 3, 4, 5, 7 },
                        Type = ContentType.Photo,
                        MimeType = "jpg"
                    };
                    contentHandler.Add(photo);
                    contentHandler.SaveChanges();

                    contentIds.Add(photo.ID);
                }

                Group testGroup_2 = await groupHandler.CreateGroupAsync(testPerson.ID, "testGroupWithTest", GroupType.GroupList);

                Blog testBlog_MultiplePhoto_3 = await blogHandler.CreateBlogAsync(testPerson.ID, "testBlogMultiplePhoto", BlogInfoAccessInfo.GroupOnly, testGroup_2.ID, contentIds);

                Blog testPhotoBlog = await blogHandler.GetByIdAsync(testBlog_MultiplePhoto_3.ID);
                BlogAccessControl testPhotoAC = await acHandler.Fetch(x => x.BlogID == testPhotoBlog.ID).FirstOrDefaultAsync();
                BlogAccessControlXGroup testPhotoACXG = await acxgHandler.Fetch(x => x.GroupID == testGroup_2.ID && x.BlogAccessControlID == testPhotoAC.ID).FirstOrDefaultAsync();
                List<BlogXContent> testPhotoBXC = await bxcHandler.Fetch(x => x.BlogID == testPhotoBlog.ID).ToListAsync();


                Assert.IsFalse(testPhotoBlog.IsDeleted);
                Assert.IsNotNull(testPhotoBlog);
                Assert.IsNotNull(testPhotoAC);
                Assert.IsNotNull(testPhotoACXG);
                Assert.IsNotNull(testPhotoBXC);

                Assert.AreEqual(testPhotoBXC.Count, 6);
                Assert.AreEqual(testPhotoBlog.PersonID, testPerson.ID);
                Assert.AreEqual(testPhotoBlog.Content, "testBlogMultiplePhoto");
                Assert.AreEqual(testPhotoAC.AccessLevel, BlogInfoAccessInfo.GroupOnly);

                foreach (var photoBXC in testPhotoBXC)
                {
                    Assert.IsNotNull(photoBXC.Content);
                    Assert.AreEqual(photoBXC.Content.Type, ContentType.Photo);
                    Assert.AreEqual(photoBXC.Content.ContentPath, "testPhotoContentPath");
                    Assert.AreEqual(photoBXC.Content.ContentBinary, new byte[] { 12, 3, 4, 5, 7 });
                }
            }
        }

        [Test]
        public async Task Test_03_ForwardBlogAsync()
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);
                BlogXBlogHandler bxbHandler = new BlogXBlogHandler(dbContext);
                ContentHandler contentHandler = new ContentHandler(dbContext);
                BlogXContentHandler bxcHandler = new BlogXContentHandler(dbContext);
                GroupHandler groupHandler = new GroupHandler(dbContext);
                BlogAccessControlHandler acHandler = new BlogAccessControlHandler(dbContext);
                BlogAccessControlXGroupHandler acxgHandler = new BlogAccessControlXGroupHandler(dbContext);

                //1. without content and test it.
                Blog testBlog = await blogHandler.CreateBlogAsync(testPerson.ID, "testWithoutContentForwardBlog", BlogInfoAccessInfo.All, null);

                Blog beForwardBlog = await blogHandler.GetByIdAsync(testBlog.ID);

                Assert.IsNotNull(beForwardBlog);
                Assert.AreEqual(beForwardBlog.Content, "testWithoutContentForwardBlog");

                //create forward blog
                Blog testForwardBlog_NoContent = await blogHandler.CreateBlogAsync(testPerson.ID, "testForwardBlogNoContent", BlogInfoAccessInfo.All, null, null, beForwardBlog.ID);

                Blog testForwardBlog = await blogHandler.GetByIdAsync(testForwardBlog_NoContent.ID);

                Assert.IsNotNull(testForwardBlog);
                Assert.IsFalse(testForwardBlog.IsDeleted);
                Assert.AreEqual(testForwardBlog.Content, "testForwardBlogNoContent");
                Assert.AreEqual(testForwardBlog.NewBlogXBlogs.Count, 1);

                Assert.IsNotNull(testForwardBlog.NewBlogXBlogs.First().NewBlog);
                Assert.IsNotNull(testForwardBlog.NewBlogXBlogs.First().BaseBlog);

                Assert.AreEqual(testForwardBlog.NewBlogXBlogs.First().NewBlog.Content, "testForwardBlogNoContent");
                Assert.AreEqual(testForwardBlog.NewBlogXBlogs.First().BaseBlog.Content, "testWithoutContentForwardBlog");

                //2. set error forward blog id and test it.
                bool isChecked = false;
                try
                {
                    Blog testErrorForwardBlogID = await blogHandler.CreateBlogAsync(testPerson.ID, "testForwardBlogNoContent", BlogInfoAccessInfo.All, null, null, 99999);
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(DisplayableException));
                    Assert.AreEqual(ex.Message, "该Blog不存在或者已经被删除");
                }
                Assert.IsTrue(isChecked);

                //3. include content and test it.
                List<long> contentIds = new List<long>();

                for (int i = 0; i < 6; i++)
                {
                    Content photo = new Content()
                    {
                        ContentPath = "testPhotoContentPath",
                        ContentBinary = new byte[] { 12, 3, 4, 5, 7 },
                        Type = ContentType.Photo,
                        MimeType = "jpg"
                    };
                    contentHandler.Add(photo);
                    contentHandler.SaveChanges();

                    contentIds.Add(photo.ID);
                }

                Blog testIncludeContentBeForwardBlog = await blogHandler.CreateBlogAsync(testPerson.ID, "testIncludeContentBeForwardBlog", BlogInfoAccessInfo.All, attachContentIds: contentIds);

                Blog testBeForwardBlog = await blogHandler.GetByIdAsync(testIncludeContentBeForwardBlog.ID);

                Assert.IsNotNull(testBeForwardBlog);
                Assert.IsFalse(testBeForwardBlog.IsDeleted);
                Assert.AreEqual(testBeForwardBlog.Content, "testIncludeContentBeForwardBlog");
                Assert.AreEqual(testBeForwardBlog.BlogXContents.Count, 6);
                Assert.IsTrue(testBeForwardBlog.BlogXContents.Any(x => x.BlogID == testBeForwardBlog.ID && x.Content.ContentPath == "testPhotoContentPath" && x.Content.Type == ContentType.Photo));

                //forward the blog.
                contentIds = new List<long>();

                Content video = new Content()
                {                    
                    ContentPath = "testVideoContentPath",
                    ContentBinary = new byte[] { 1, 3, 5, 6, 99 },
                    Type = ContentType.Video,
                    MimeType = "jpg"
                };
                contentHandler.Add(video);
                contentHandler.SaveChanges();

                contentIds.Add(video.ID);

                Blog testIncludeContentForwardBlog = await blogHandler.CreateBlogAsync(testPerson.ID, "testIncludeContentForwardBlog", BlogInfoAccessInfo.All, attachContentIds: contentIds, forwardBlogId: testBeForwardBlog.ID);

                Blog testForwardBlogIncludeContent = await blogHandler.GetByIdAsync(testIncludeContentForwardBlog.ID);

                Assert.IsNotNull(testForwardBlogIncludeContent);
                Assert.IsFalse(testForwardBlogIncludeContent.IsDeleted);
                Assert.AreEqual(testForwardBlogIncludeContent.Content, "testIncludeContentForwardBlog");
                Assert.AreEqual(testForwardBlogIncludeContent.BlogXContents.Count, 1);
                Assert.IsTrue(testForwardBlogIncludeContent.BlogXContents.Any(x => x.BlogID == testForwardBlogIncludeContent.ID && x.Content.ContentPath == "testVideoContentPath" && x.Content.Type == ContentType.Video));

                Assert.AreEqual(testForwardBlogIncludeContent.NewBlogXBlogs.Count, 1);

                Assert.IsNotNull(testForwardBlogIncludeContent.NewBlogXBlogs.First().NewBlog);
                Assert.IsNotNull(testForwardBlogIncludeContent.NewBlogXBlogs.First().BaseBlog);

                Assert.AreEqual(testForwardBlogIncludeContent.NewBlogXBlogs.First().NewBlog.Content, "testIncludeContentForwardBlog");
                Assert.AreEqual(testForwardBlogIncludeContent.NewBlogXBlogs.First().BaseBlog.Content, "testIncludeContentBeForwardBlog");

                Assert.AreEqual(testForwardBlogIncludeContent.NewBlogXBlogs.First().NewBlog.BlogXContents.Count, 1);
                Assert.AreEqual(testForwardBlogIncludeContent.NewBlogXBlogs.First().BaseBlog.BlogXContents.Count, 6);

                foreach (var newBlogContent in testForwardBlogIncludeContent.NewBlogXBlogs.First().NewBlog.BlogXContents)
                {
                    Assert.AreEqual(newBlogContent.Blog.ID, testForwardBlogIncludeContent.ID);
                    Assert.AreEqual(newBlogContent.Content.ContentPath, "testVideoContentPath");
                    Assert.AreEqual(newBlogContent.Content.ContentBinary, new byte[] { 1, 3, 5, 6, 99 });
                }

                foreach (var baseBlogContent in testForwardBlogIncludeContent.NewBlogXBlogs.First().BaseBlog.BlogXContents)
                {
                    Assert.AreEqual(baseBlogContent.Blog.ID, testBeForwardBlog.ID);
                    Assert.AreEqual(baseBlogContent.Content.ContentPath, "testPhotoContentPath");
                    Assert.AreEqual(baseBlogContent.Content.ContentBinary, new byte[] { 12, 3, 4, 5, 7 });
                }
            }
        }

        [Test]
        public async Task Test_04_Normal_GetBlogsAsync()
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);
                GroupHandler groupHandler = new GroupHandler(dbContext);
                GroupMemberHandler gmHandler = new GroupMemberHandler(dbContext);
                PersonHandler perHandler = new PersonHandler(dbContext);
                PersonXPersonHandler pxpHandler = new PersonXPersonHandler(dbContext);

                //1. test normal.
                bool isChecked = false;
                try
                {
                    List<Blog> invalidPersonIdBlogs = await blogHandler.GetBlogsAsync(999999999);
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.Message, "该用户不存在");
                }
                Assert.IsTrue(isChecked);

                Person master = CreatePerson("TestMasterPer", "TestMasterMind");

                Person mary = CreatePerson("TestMary", "TestMary");
                Person nick = CreatePerson("TestNick", "TestNick");
                Person tony = CreatePerson("TestTony", "TestTony");

                //1. create some blog and test it.
                await blogHandler.CreateBlogAsync(master.ID, "TestContentByMaster", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(mary.ID, "TestContentByMaryOne", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(mary.ID, "TestContentByMaryTwo", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(nick.ID, "TestContentByNickOne", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(tony.ID, "TestContentByTonyOne", BlogInfoAccessInfo.All);

                Follow(master.ID, mary.ID, nick.ID, tony.ID);
                
                List<Blog> blogs = await blogHandler.GetBlogsAsync(master.ID);
                
                Assert.AreEqual(blogs.Count, 5);
                Assert.AreEqual(blogs.Count(x => x.PersonID == mary.ID), 2);
                Assert.AreEqual(blogs.Count(x => x.PersonID == nick.ID), 1);
                Assert.AreEqual(blogs.Count(x => x.PersonID == tony.ID), 1);
                Assert.AreEqual(blogs.Count(x => x.PersonID == master.ID), 1);

                //2. add a group and add some group member test it.
                Group masterGroup = new Group()
                {
                    PersonID = master.ID,
                    Name = "TestMasterGroup",
                    Type = GroupType.GroupList
                };
                groupHandler.Add(masterGroup);
                groupHandler.SaveChanges();

                GroupMember GroupMemberByMary = new GroupMember()
                {
                    GroupID = masterGroup.ID,
                    PersonID = mary.ID
                };
                GroupMember GroupMemberByNick = new GroupMember()
                {
                    GroupID = masterGroup.ID,
                    PersonID = nick.ID
                };
                gmHandler.Add(GroupMemberByMary);
                gmHandler.Add(GroupMemberByNick);
                gmHandler.SaveChanges();

                //3. test get blog by group.
                blogs = await blogHandler.GetBlogsAsync(master.ID, masterGroup.ID);

                Assert.AreEqual(blogs.Count, 4);
                Assert.AreEqual(blogs.Count(x => x.PersonID == mary.ID), 2);
                Assert.AreEqual(blogs.Count(x => x.PersonID == nick.ID), 1);
                Assert.AreEqual(blogs.Count(x => x.PersonID == master.ID), 1);

                //4. add shield group and test it.
                Person mike = CreatePerson("TestMike", "TestMike");
                Person yoyo = CreatePerson("TestYOYO", "TestYOYO");
                Person pipi = CreatePerson("TestPIPI", "TestPIPI");
                Person poko = CreatePerson("TestPoko", "TestPoko");

                Follow(master.ID, mike.ID, yoyo.ID, pipi.ID, poko.ID);

                await blogHandler.CreateBlogAsync(mike.ID, "TestContentByMikeOne", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(mike.ID, "TestContentByMikeTwo", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(yoyo.ID, "TestContentByYoyoOne", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(yoyo.ID, "TestContentByYoyoTwo", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(pipi.ID, "TestContentByPipiOne", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(poko.ID, "TestContentByPokoOne", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(poko.ID, "TestContentByPokoTwo", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(poko.ID, "TestContentByPokoThree", BlogInfoAccessInfo.All);

                Group masterShieldGroup = new Group()
                {
                    PersonID = master.ID,
                    Name = "TestMasterShield",
                    Type = GroupType.ShieldList
                };
                groupHandler.Add(masterShieldGroup);
                groupHandler.SaveChanges();

                GroupMember GroupMemberByPoko = new GroupMember()
                {
                    GroupID = masterShieldGroup.ID,
                    PersonID = poko.ID
                };

                gmHandler.Add(GroupMemberByPoko);
                gmHandler.SaveChanges();

                blogs = await blogHandler.GetBlogsAsync(master.ID);

                Assert.AreEqual(blogs.Count, 10);

                foreach (var blog in blogs)
                {
                    Assert.AreNotEqual(blog.PersonID, poko.ID);
                    Assert.AreNotEqual(blog.Content, "TestContentByPokoOne");
                    Assert.AreNotEqual(blog.Content, "TestContentByPokoTwo");
                    Assert.AreNotEqual(blog.Content, "TestContentByPokoThree");
                }

                //5. add group member to normal group and test it.
                GroupMember GroupMemberByPoko_Normal = new GroupMember()
                {
                    GroupID = masterGroup.ID,
                    PersonID = poko.ID
                };
                gmHandler.Add(GroupMemberByMary);

                blogs = await blogHandler.GetBlogsAsync(master.ID, masterGroup.ID);

                Assert.AreEqual(blogs.Count, 4);
                Assert.AreEqual(blogs.Count(x => x.PersonID == mary.ID), 2);
                Assert.AreEqual(blogs.Count(x => x.PersonID == nick.ID), 1);
                Assert.AreEqual(blogs.Count(x => x.PersonID == master.ID), 1);
            }
        }

        [Test]
        public async Task Test_05_AccessControl_GetBlogsAsync()
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);
                GroupHandler groupHandler = new GroupHandler(dbContext);
                GroupMemberHandler gmHandler = new GroupMemberHandler(dbContext);
                PersonHandler perHandler = new PersonHandler(dbContext);
                PersonXPersonHandler pxpHandler = new PersonXPersonHandler(dbContext);

                Person faker = CreatePerson("TestFaker", "TestFaker");
                Person marin = CreatePerson("TestMarin", "TestMarin");
                Person deft = CreatePerson("TestDeft", "TestDeft");

                Follow(faker.ID, marin.ID, deft.ID);

                //1. test access info MySelfOnly.
                await blogHandler.CreateBlogAsync(faker.ID, "TestContentByFakerOne", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(marin.ID, "TestContentByMarinOne", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(marin.ID, "TestContentByMarinTwo", BlogInfoAccessInfo.MyselfOnly);
                await blogHandler.CreateBlogAsync(deft.ID, "TestContentByDeftOne", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(deft.ID, "TestContentByDeftTwo", BlogInfoAccessInfo.MyselfOnly);

                List<Blog> blogs = await blogHandler.GetBlogsAsync(faker.ID);

                Assert.AreEqual(blogs.Count, 3);

                foreach (var blog in blogs)
                {
                    Assert.AreNotEqual(blog.Content, "TestContentByMarinTwo");
                    Assert.AreNotEqual(blog.Content, "TestContentByDeftTwo");
                }

                //2. test access info GroupOnly.
                Person paul = CreatePerson("TestPaul", "TestPaul");
                Person judy = CreatePerson("TestJudy", "TestJudy");
                Person anne = CreatePerson("TestAnne", "TestAnne");
                Person alisa = CreatePerson("TestAlisa", "TestAlisa");

                Follow(paul.ID, judy.ID, anne.ID, alisa.ID);

                //judy group not include paul.
                Group judyGroup = await groupHandler.CreateGroupAsync(judy.ID, "TestJudyGroup", GroupType.GroupList);

                //anne group include paul.
                Group anneGroup = await groupHandler.CreateGroupAsync(anne.ID, "TestAnneGroup", GroupType.GroupList);

                GroupMember groupMemberByAnne = new GroupMember()
                {
                    GroupID = anneGroup.ID,
                    PersonID = paul.ID
                };
                gmHandler.Add(groupMemberByAnne);
                gmHandler.SaveChanges();

                //alisa group include judy, anne but not paul.
                Group alisaGroup = await groupHandler.CreateGroupAsync(alisa.ID, "TestAlisaGroup", GroupType.GroupList);

                GroupMember groupMemberByAlisaOne = new GroupMember()
                {
                    GroupID = alisaGroup.ID,
                    PersonID = judy.ID
                };
                GroupMember groupMemberByAlisaTwo = new GroupMember()
                {
                    GroupID = alisaGroup.ID,
                    PersonID = anne.ID
                };

                gmHandler.Add(groupMemberByAlisaOne);
                gmHandler.Add(groupMemberByAlisaTwo);
                gmHandler.SaveChanges();

                await blogHandler.CreateBlogAsync(paul.ID, "TestContentByPaul", BlogInfoAccessInfo.MyselfOnly);
                await blogHandler.CreateBlogAsync(judy.ID, "TestContentByJudyOne", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(judy.ID, "TestContentByJudyTwo", BlogInfoAccessInfo.GroupOnly, judyGroup.ID);
                await blogHandler.CreateBlogAsync(anne.ID, "TestContentByAnne", BlogInfoAccessInfo.GroupOnly, anneGroup.ID);
                await blogHandler.CreateBlogAsync(alisa.ID, "TestContentByAlisa", BlogInfoAccessInfo.GroupOnly, alisaGroup.ID);

                blogs = await blogHandler.GetBlogsAsync(paul.ID);

                Assert.AreEqual(blogs.Count, 3);

                foreach (var blog in blogs)
                {
                    Assert.AreNotEqual(blog.Content, "TestContentByJudyTwo");
                    Assert.AreNotEqual(blog.Content, "TestContentByAlisa");
                }

                //3. test access info FriendOnly.
                Person sam = CreatePerson("TestSam", "TestSam");
                Person joan = CreatePerson("TestJoan", "TestJoan");
                Person lily = CreatePerson("TestLily", "TestLily");

                Follow(sam.ID, joan.ID, lily.ID);

                //3.1 test joan and sam is friend but not lily.
                Follow(joan.ID, sam.ID);

                await blogHandler.CreateBlogAsync(joan.ID, "TestContentByJoanOne", BlogInfoAccessInfo.FriendOnly);
                await blogHandler.CreateBlogAsync(joan.ID, "TestContentByJoanTwo", BlogInfoAccessInfo.All);
                await blogHandler.CreateBlogAsync(lily.ID, "TestContentByLilyOne", BlogInfoAccessInfo.FriendOnly);
                await blogHandler.CreateBlogAsync(lily.ID, "TestContentByLilyTwo", BlogInfoAccessInfo.FriendOnly);

                blogs = await blogHandler.GetBlogsAsync(sam.ID);

                Assert.AreEqual(blogs.Count, 2);

                foreach (var blog in blogs)
                {
                    Assert.AreNotEqual(blog.Content, "TestContentByLilyOne");
                    Assert.AreNotEqual(blog.Content, "TestContentByLilyTwo");
                }

                //3.2 now lily follow sam too.
                Follow(lily.ID, sam.ID);

                blogs = await blogHandler.GetBlogsAsync(sam.ID);

                Assert.AreEqual(blogs.Count, 4);
            }
        }

        #region Private Function. Create Person.
        private Person CreatePerson(string realName, string nickName)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                PersonHandler perHandler = new PersonHandler(dbContext);

                Person p = new Person();

                p = new Person();
                p.RealName = realName;
                p.NickName = nickName;
                p.RealNameAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                p.SexualTrendAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                p.MaritalStatusAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                p.QQAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                p.DOBAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                p.BloodTypeAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                p.HomePageAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                perHandler.Add(p);
                perHandler.SaveChanges();

                return p;
            }
        }
        #endregion

        #region Private Function. Follow.
        private void Follow(long selfPerId, params long[] followingIds)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                PersonXPersonHandler pxpHandler = new PersonXPersonHandler(dbContext);

                foreach (var followingId in followingIds)
	            {
                    PersonXPerson pxp = new PersonXPerson()
                    {
                        FollowerID = selfPerId,
                        FollowingID = followingId
                    };
                    pxpHandler.Add(pxp);
	            }
                pxpHandler.SaveChanges();
            }
        }
        #endregion
    }
}
