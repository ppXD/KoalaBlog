using KoalaBlog.BLL.Handlers;
using KoalaBlog.DAL;
using KoalaBlog.Entity.Models;
using KoalaBlog.Entity.Models.Enums;
using KoalaBlog.Framework.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Test
{
    class TestComment
    {
        private Person testPerson;
        private Person commentPerson;

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

                commentPerson = new Person();
                commentPerson.RealName = "testCommentRealName";
                commentPerson.NickName = "testCommentPerson";
                commentPerson.RealNameAccessLevel = PersonInfoAccessInfo.All;
                commentPerson.SexualTrendAccessLevel = PersonInfoAccessInfo.All;
                commentPerson.MaritalStatusAccessLevel = PersonInfoAccessInfo.All;
                commentPerson.QQAccessLevel = PersonInfoAccessInfo.All;
                commentPerson.DOBAccessLevel = PersonInfoAccessInfo.All;
                commentPerson.BloodTypeAccessLevel = PersonInfoAccessInfo.All;
                commentPerson.HomePageAccessLevel = PersonInfoAccessInfo.All;
                perHandler.Add(commentPerson);
                perHandler.SaveChanges();
            }
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            TestUtil.CleanUpData();
        }

        [Test]
        public async Task Test_01_AddCommentWithoutContent()
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);
                GroupHandler groupHandler = new GroupHandler(dbContext);
                GroupMemberHandler gmHandler = new GroupMemberHandler(dbContext);
                CommentHandler commentHandler = new CommentHandler(dbContext);

                Person sam = CreatePerson("TestSam", "TestSam", AllowablePersonForComment.All, true);

                Blog testBlog = await blogHandler.CreateBlogAsync(sam.ID, "TestCommentBlog", BlogInfoAccessInfo.MyselfOnly);

                Person shelly = CreatePerson("TestShelly", "TestShelly", AllowablePersonForComment.All, true);

                //1. normal test.
                Comment testComment = await commentHandler.AddCommentAsync(shelly.ID, testBlog.ID, "hello, i am a comment test");

                Comment testNormalComment = await commentHandler.GetByIdAsync(testComment.ID);

                Assert.IsNotNull(testNormalComment);
                Assert.AreEqual(testNormalComment.PersonID, shelly.ID);
                Assert.AreEqual(testNormalComment.BlogID, testBlog.ID);
                Assert.AreEqual(testNormalComment.Content, "hello, i am a comment test");

                //2. set error blog id and test it.
                bool isChecked = false;
                try
                {
                    Comment testComment_1 = await commentHandler.AddCommentAsync(commentPerson.ID, 12313121, "hello, i am a comment test");
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(DisplayableException));
                    Assert.AreEqual(ex.Message, "要评论的Blog不存在或者已经被删除");
                }
                Assert.IsTrue(isChecked);

                //3. test black list comment.
                Group blackListGroup = new Group()
                {
                    PersonID = sam.ID,
                    Name = "TestBlackList",
                    Type = GroupType.BlackList
                };
                groupHandler.Add(blackListGroup);
                groupHandler.SaveChanges();

                GroupMember GroupMemberByBlack = new GroupMember()
                {
                    GroupID = blackListGroup.ID,
                    PersonID = commentPerson.ID
                };

                gmHandler.Add(GroupMemberByBlack);
                gmHandler.SaveChanges();

                isChecked = false;
                try
                {
                    Comment testComment_2 = await commentHandler.AddCommentAsync(commentPerson.ID, testBlog.ID, "hello, i am a comment test 4 black list.");
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(DisplayableException));
                    Assert.AreEqual(ex.Message, "由于用户设置，你无法回复评论。");
                }
                Assert.IsTrue(isChecked);

                //4. remove black list member.
                gmHandler.MarkAsDeleted(GroupMemberByBlack);
                gmHandler.SaveChanges();
            }
        }

        [Test]
        public async Task Test_02_AddCommentIncludeContent()
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);
                ContentHandler contentHandler = new ContentHandler(dbContext);
                CommentHandler commentHandler = new CommentHandler(dbContext);

                Person jay = CreatePerson("TestJay", "TestJay", AllowablePersonForComment.All, true);

                Blog testBlog = await blogHandler.CreateBlogAsync(jay.ID, "TestCommentBlog", BlogInfoAccessInfo.MyselfOnly);

                //1. normal test.
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

                Comment testComment = await commentHandler.AddCommentAsync(commentPerson.ID, testBlog.ID, "hello, i am a comment include content", photoContentIds: contentIds);

                Comment testNormalComment = await commentHandler.GetByIdAsync(testComment.ID);

                Assert.IsNotNull(testNormalComment);
                Assert.AreEqual(testNormalComment.PersonID, commentPerson.ID);
                Assert.AreEqual(testNormalComment.BlogID, testBlog.ID);
                Assert.AreEqual(testNormalComment.Content, "hello, i am a comment include content");
                Assert.AreEqual(testNormalComment.CommentXContents.Count, 1);
                Assert.AreEqual(testNormalComment.CommentXContents.First().Content.ContentPath, "testPath");
                Assert.AreEqual(testNormalComment.CommentXContents.First().Content.Type, ContentType.Photo);
                Assert.AreEqual(testNormalComment.CommentXContents.First().Content.ContentBinary, new byte[] { 1, 3, 5 });

                //2. test multiple content.
                contentIds = new List<long>();

                for (int i = 0; i < 5; i++)
                {
                    Content photoContent = new Content()
                    {
                        ContentPath = "testMultiplePath",
                        ContentBinary = new byte[] { 3, 6, 9 },
                        Type = ContentType.Photo,
                        MimeType = "jpg"
                    };
                    contentHandler.Add(photoContent);
                    contentHandler.SaveChanges();

                    contentIds.Add(photoContent.ID);
                }

                Comment testComment_1 = await commentHandler.AddCommentAsync(commentPerson.ID, testBlog.ID, "hello, i am a multiple comment include content oh yes", photoContentIds: contentIds);

                Comment testMultipleComment = await commentHandler.GetByIdAsync(testComment_1.ID);

                Assert.IsNotNull(testMultipleComment);
                Assert.AreEqual(testMultipleComment.PersonID, commentPerson.ID);
                Assert.AreEqual(testMultipleComment.BlogID, testBlog.ID);
                Assert.AreEqual(testMultipleComment.Content, "hello, i am a multiple comment include content oh yes");
                Assert.AreEqual(testMultipleComment.CommentXContents.Count, 5);
                Assert.AreEqual(testMultipleComment.CommentXContents.First().Content.ContentPath, "testMultiplePath");
                Assert.AreEqual(testMultipleComment.CommentXContents.First().Content.Type, ContentType.Photo);
                Assert.AreEqual(testMultipleComment.CommentXContents.First().Content.ContentBinary, new byte[] { 3, 6, 9 });

                //3. set the content type is video or music and test it.
                bool isChecked = false;
                try
                {
                    contentIds = new List<long>();

                    for (int i = 0; i < 5; i++)
                    {
                        Content photoContent = new Content()
                        {
                            ContentPath = "testMultiplePath",
                            ContentBinary = new byte[] { 3, 6, 9 },                           
                            Type = i % 2 == 0 ? ContentType.Music : ContentType.Video,
                            MimeType = "jpg"
                        };
                        contentHandler.Add(photoContent);
                        contentHandler.SaveChanges();

                        contentIds.Add(photoContent.ID);
                    }
                    Comment testComment_2 = await commentHandler.AddCommentAsync(commentPerson.ID, testBlog.ID, "hello, i am a multiple comment include content but type not photo", photoContentIds: contentIds);
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(DisplayableException));
                    Assert.AreEqual(ex.Message, "评论只能附件图片");
                }
                Assert.IsTrue(isChecked);
            }
        }

        [Test]
        public async Task Test_03_AddCommentIncludeBaseComment()
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);
                ContentHandler contentHandler = new ContentHandler(dbContext);
                CommentHandler commentHandler = new CommentHandler(dbContext);
                CommentXCommentHandler cxcHandler = new CommentXCommentHandler(dbContext);
                CommentXContentHandler ccHandler = new CommentXContentHandler(dbContext);

                Person rain = CreatePerson("TestRain", "TestRain", AllowablePersonForComment.All, true);

                Blog testBlog = await blogHandler.CreateBlogAsync(rain.ID, "TestCommentBlog", BlogInfoAccessInfo.MyselfOnly);

                //1. test normal.
                List<long> contentIds = new List<long>();

                Content content = new Content()
                {
                    ContentPath = "testBeCommentPath",
                    ContentBinary = new byte[] { 1, 3, 5 },
                    Type = ContentType.Photo,
                    MimeType = "jpg"
                };
                contentHandler.Add(content);
                contentHandler.SaveChanges();

                contentIds.Add(content.ID);

                Comment beComment = await commentHandler.AddCommentAsync(commentPerson.ID, testBlog.ID, "hey, i am be comment test.", photoContentIds: contentIds);

                Comment test_beComment = await commentHandler.GetByIdAsync(beComment.ID);

                Assert.IsNotNull(test_beComment);
                Assert.AreEqual(test_beComment.PersonID, commentPerson.ID);
                Assert.AreEqual(test_beComment.BlogID, testBlog.ID);
                Assert.AreEqual(test_beComment.Content, "hey, i am be comment test.");
                Assert.AreEqual(test_beComment.CommentXContents.Count, 1);
                Assert.AreEqual(test_beComment.CommentXContents.First().Content.ContentPath, "testBeCommentPath");
                Assert.AreEqual(test_beComment.CommentXContents.First().Content.Type, ContentType.Photo);
                Assert.AreEqual(test_beComment.CommentXContents.First().Content.ContentBinary, new byte[] { 1, 3, 5 });

                contentIds = new List<long>();

                Content newCommentContent = new Content()
                {
                    ContentPath = "testBeCommentPath",
                    ContentBinary = new byte[] { 1, 3, 5 },
                    Type = ContentType.Photo,
                    MimeType = "jpg"
                };
                contentHandler.Add(newCommentContent);
                contentHandler.SaveChanges();

                contentIds.Add(newCommentContent.ID);

                Comment newComment = await commentHandler.AddCommentAsync(commentPerson.ID, testBlog.ID, "hey, i am new comment test.", photoContentIds: contentIds, baseCommentId: test_beComment.ID);

                Comment test_newComment = await commentHandler.GetByIdAsync(newComment.ID);

                Assert.IsNotNull(test_newComment);
                Assert.AreEqual(test_newComment.PersonID, commentPerson.ID);
                Assert.AreEqual(test_newComment.BlogID, testBlog.ID);
                Assert.AreEqual(test_newComment.Content, "hey, i am new comment test.");
                Assert.AreEqual(test_newComment.CommentXContents.Count, 1);
                Assert.AreEqual(test_newComment.NewCommentXComments.Count, 1);

                Assert.AreEqual(test_newComment.CommentXContents.First().Content.ContentPath, "testBeCommentPath");
                Assert.AreEqual(test_newComment.CommentXContents.First().Content.Type, ContentType.Photo);
                Assert.AreEqual(test_newComment.CommentXContents.First().Content.ContentBinary, new byte[] { 1, 3, 5 });

                Assert.AreEqual(test_newComment.NewCommentXComments.First().BaseComment.ID, test_beComment.ID);
                Assert.AreEqual(test_newComment.NewCommentXComments.First().NewComment.ID, test_newComment.ID);
                Assert.AreEqual(test_newComment.NewCommentXComments.First().BaseComment.Content, "hey, i am be comment test.");
                Assert.AreEqual(test_newComment.NewCommentXComments.First().NewComment.Content, "hey, i am new comment test.");

                //2. delete a comment and test it.
                bool isChecked = false;
                try
                {
                    long tmpCommentId = test_beComment.ID;

                    //delete comment.

                    //2.1 first, delete all navigation for comment.
                    for (int i = 0; i < test_beComment.BaseCommentXComments.Count; i++)
                    {
                        cxcHandler.MarkAsDeleted(test_beComment.BaseCommentXComments.ElementAt(i));
                    }
                    await cxcHandler.SaveChangesAsync();

                    for (int i = 0; i < test_beComment.CommentXContents.Count; i++)
                    {
                        ccHandler.MarkAsDeleted(test_beComment.CommentXContents.ElementAt(i));
                    }

                    await ccHandler.SaveChangesAsync();

                    //2.2 then, delete comment.
                    commentHandler.MarkAsDeleted(test_beComment);
                    await commentHandler.SaveChangesAsync();

                    Comment test_notExistComment = await commentHandler.AddCommentAsync(commentPerson.ID, testBlog.ID, "hey, i am new comment test.", baseCommentId: tmpCommentId);
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(DisplayableException));
                    Assert.AreEqual(ex.Message, "此评论不存在或者已经被删除");                  
                }
                Assert.IsTrue(isChecked);
            }
        }

        [Test]
        public async Task Test_04_AddCommentAllowableCheck()
        {
            long eddyId = 0;
            long eastId = 0;
            long blogId = 0;

            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);
                ContentHandler contentHandler = new ContentHandler(dbContext);
                CommentHandler commentHandler = new CommentHandler(dbContext);

                //1. check allow follower comment.
                Person eddy = CreatePerson("TestEddy", "TestEddy", AllowablePersonForComment.FollowerOnly, true);
                Person east = CreatePerson("TestEast", "TestEast", AllowablePersonForComment.FollowerOnly, true);

                Blog testBlog = await blogHandler.CreateBlogAsync(eddy.ID, "TestCommentBlog", BlogInfoAccessInfo.MyselfOnly);

                bool isChecked = false;

                try
                {
                    Comment comment = await commentHandler.AddCommentAsync(east.ID, testBlog.ID, "hey, i am new comment test.");
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(DisplayableException));
                    Assert.AreEqual(ex.Message, "由于用户设置，你无法回复评论。");
                }

                Assert.IsTrue(isChecked);

                eddyId = eddy.ID;
                eastId = east.ID;
                blogId = testBlog.ID;
            }

            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);
                ContentHandler contentHandler = new ContentHandler(dbContext);
                CommentHandler commentHandler = new CommentHandler(dbContext);

                //1.1 eddy follow east, and test it again.
                Follow(eddyId, eastId);

                Comment comment_1 = await commentHandler.AddCommentAsync(eastId, blogId, "hey, i am new comment test.");

                Assert.IsNotNull(comment_1);
            }

            long alienId = 0;
            long paulId = 0;
            long fansBlogId = 0;

            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);
                ContentHandler contentHandler = new ContentHandler(dbContext);
                CommentHandler commentHandler = new CommentHandler(dbContext);

                //1. check allow fans comment.
                Person alien = CreatePerson("TestAlien", "TestAlien", AllowablePersonForComment.FansOnly, true);
                Person paul = CreatePerson("TestPaul", "TestPaul", AllowablePersonForComment.FollowerOnly, true);

                Blog testBlog = await blogHandler.CreateBlogAsync(alien.ID, "TestCommentBlog", BlogInfoAccessInfo.MyselfOnly);

                bool isChecked = false;

                try
                {
                    Comment comment = await commentHandler.AddCommentAsync(paul.ID, testBlog.ID, "hey, i am new comment test.");
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(DisplayableException));
                    Assert.AreEqual(ex.Message, "由于用户设置，你无法回复评论。");
                }

                Assert.IsTrue(isChecked);

                alienId = alien.ID;
                paulId = paul.ID;
                fansBlogId = testBlog.ID;
            }

            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);
                ContentHandler contentHandler = new ContentHandler(dbContext);
                CommentHandler commentHandler = new CommentHandler(dbContext);

                Follow(paulId, alienId);

                Comment comment_1 = await commentHandler.AddCommentAsync(paulId, fansBlogId, "hey, i am new comment test.");

                Assert.IsNotNull(comment_1);
            }

            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);
                ContentHandler contentHandler = new ContentHandler(dbContext);
                CommentHandler commentHandler = new CommentHandler(dbContext);

                //1. check allow fans comment.
                Person alien = CreatePerson("TestAlien", "TestAlien", AllowablePersonForComment.All, false);
                Person paul = CreatePerson("TestPaul", "TestPaul", AllowablePersonForComment.FollowerOnly, true);

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

                Blog testBlog = await blogHandler.CreateBlogAsync(alien.ID, "TestCommentBlog", BlogInfoAccessInfo.MyselfOnly);

                bool isChecked = false;

                try
                {
                    Comment comment = await commentHandler.AddCommentAsync(paul.ID, testBlog.ID, "hey, i am new comment test.", photoContentIds: contentIds);
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(DisplayableException));
                    Assert.AreEqual(ex.Message, "由于用户设置，你回复评论无法添加图片。");
                }

                Assert.IsTrue(isChecked);
            }
        }

        #region Private Function. Create Person.
        private Person CreatePerson(string realName, string nickName, AllowablePersonForComment allowablePersonForComment, bool allowCommentAttachContent)
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
                p.AllowablePersonForComment = allowablePersonForComment;
                p.AllowCommentAttachContent = allowCommentAttachContent;
                perHandler.Add(p);
                perHandler.SaveChanges();

                return p;
            }
        }
        #endregion

        #region Private Function. Follow.
        private void Follow(long selfPerId, params long[] followingIds)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
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
