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
using System.Data.Entity;

namespace KoalaBlog.BLL.Test
{
    class TestPerson
    {
        private UserAccount testUA1;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            TestUtil.CleanUpData();

            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                UserAccountHandler uaHandler = new UserAccountHandler(dbContext);

                testUA1 = new UserAccount();
                testUA1.UserName = "testUserAccount";
                testUA1.PasswordSalt = "testSalt1";
                testUA1.Password = "testPassword";
                testUA1.Email = "test@test.com";
                testUA1.LastLogon = DateTime.Now;
                testUA1.EmailConfirmed = true;
                testUA1.Status = UserAccount.STATUS_ACTIVE;
                uaHandler.Add(testUA1);
                uaHandler.SaveChanges();
            }
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            TestUtil.CleanUpData();
        }

        [Test]
        public async Task Test_01_CreatePersonAsync()
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                PersonHandler perHandler = new PersonHandler(dbContext);
                UserAccountXPersonHandler uaxpHandler = new UserAccountXPersonHandler(dbContext);

                //1. Test normal create person
                Person p = await perHandler.CreatePersonAsync(testUA1);

                Assert.IsNotNull(p);
                Assert.AreEqual(p.NickName, "testUserAccount");
                Assert.AreEqual(p.Gender, null);
                Assert.AreEqual(p.RealNameAccessLevel, PersonInfoAccessInfo.MyselfOnly);
                Assert.AreEqual(p.SexualTrendAccessLevel, PersonInfoAccessInfo.MyselfOnly);
                Assert.AreEqual(p.DOBAccessLevel, PersonInfoAccessInfo.MyselfOnly);

                //2. Get the UserAccountXPerson and test
                UserAccountXPerson uaxp = await uaxpHandler.LoadByUserAccountIDAsync(testUA1.ID);
                Assert.IsNotNull(uaxp);
                Assert.AreEqual(uaxp.PersonID, p.ID);
                Assert.AreEqual(uaxp.UserAccountID, testUA1.ID);

                //3. Give the null parameter and check it.
                bool isChecked = false;
                try
                {
                    Person per = await perHandler.CreatePersonAsync(null);
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(AssertException));
                    Assert.AreEqual(ex.Message, "UserAccount can't be null");
                }
                Assert.IsTrue(isChecked);

                //4. Give the error user account and check it.
                isChecked = false;
                try
                {
                    UserAccount ua = new UserAccount() { ID = 99999 };
                    Person per = await perHandler.CreatePersonAsync(ua);
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(AssertException));
                    Assert.AreEqual(ex.Message, "This user account doesn't exist");
                }
                Assert.IsTrue(isChecked);

                //5. Give the same user account and check it.
                isChecked = false;
                try
                {
                    Person per = await perHandler.CreatePersonAsync(testUA1);
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(AssertException));
                    Assert.AreEqual(ex.Message, "Existing relationships");
                }
                Assert.IsTrue(isChecked);
            }
        }

        [Test]
        public async Task Test_02_ModifyPersonNickNameAsync()
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                //1. Create a test person obj.
                PersonHandler perHandler = new PersonHandler(dbContext);
                Person p = new Person()
                {
                    RealName = "testMike",
                    NickName = "testIAMMike",
                    Gender = Gender.Male,
                    DOB = new DateTime(2015, 7, 15),
                    RealNameAccessLevel = PersonInfoAccessInfo.MyselfOnly,
                    SexualTrendAccessLevel = PersonInfoAccessInfo.MyselfOnly,
                    MaritalStatusAccessLevel = PersonInfoAccessInfo.MyselfOnly,
                    QQAccessLevel = PersonInfoAccessInfo.MyselfOnly,
                    DOBAccessLevel = PersonInfoAccessInfo.MyselfOnly,
                    BloodTypeAccessLevel = PersonInfoAccessInfo.MyselfOnly,
                    HomePageAccessLevel = PersonInfoAccessInfo.MyselfOnly
                };
                perHandler.Add(p);
                await perHandler.SaveChangesAsync();

                Assert.AreNotEqual(p.ID, 0);

                //2. Modify the person nick name and test it.
                bool isSucceed = await perHandler.ModifyPersonNickNameAsync(p.ID, "testIAMShy");
                Assert.IsTrue(isSucceed);

                //2.1 Get the person by id
                Person modifiedPerson = await perHandler.GetByIdAsync(p.ID);

                Assert.IsNotNull(modifiedPerson);
                Assert.AreEqual(modifiedPerson.NickName, "testIAMShy");
                Assert.AreEqual(modifiedPerson.RealName, "testMike");
                Assert.AreEqual(modifiedPerson.Gender, Gender.Male);
                Assert.AreEqual(modifiedPerson.DOB, new DateTime(2015, 7, 15));
                Assert.AreEqual(modifiedPerson.RealNameAccessLevel, PersonInfoAccessInfo.MyselfOnly);

                //3. Give the error parameter and test it.
                bool isChecked = false;
                try
                {
                    isSucceed = await perHandler.ModifyPersonNickNameAsync(0, "testSorryShy");
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(AssertException));
                    Assert.AreEqual(ex.Message, "personId must be greater than zero");
                }
                Assert.IsTrue(isChecked);

                isChecked = false;
                try
                {
                    isSucceed = await perHandler.ModifyPersonNickNameAsync(0, "");
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(AssertException));
                    AssertException assertException = ex as AssertException;
                    Assert.IsNotNull(assertException.ExceptionMessageList);
                    Assert.AreEqual(assertException.ExceptionMessageList.Count, 2);
                    Assert.AreEqual(assertException.ExceptionMessageList[0], "personId must be greater than zero");
                    Assert.AreEqual(assertException.ExceptionMessageList[1], "nickName can't be empty");
                }
                Assert.IsTrue(isChecked);

                isChecked = false;
                try
                {
                    isSucceed = await perHandler.ModifyPersonNickNameAsync(99999999, "testIAMShy");
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(AssertException));
                    Assert.AreEqual(ex.Message, "This person doesn't exist");
                }
                Assert.IsTrue(isChecked);
            }
        }

        [Test]
        public async Task Test_03_ModifyPersonInfoAsync()
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                //1. Create a test person obj.
                PersonHandler perHandler = new PersonHandler(dbContext);
                Person p = new Person()
                {
                    RealName = "testJay",
                    NickName = "testMayBeJay",
                    Gender = Gender.Female,
                    DOB = new DateTime(2015, 8, 23),
                    RealNameAccessLevel = PersonInfoAccessInfo.MyselfOnly,
                    SexualTrendAccessLevel = PersonInfoAccessInfo.MyselfOnly,
                    MaritalStatusAccessLevel = PersonInfoAccessInfo.MyselfOnly,
                    QQAccessLevel = PersonInfoAccessInfo.MyselfOnly,
                    DOBAccessLevel = PersonInfoAccessInfo.MyselfOnly,
                    BloodTypeAccessLevel = PersonInfoAccessInfo.MyselfOnly,
                    HomePageAccessLevel = PersonInfoAccessInfo.MyselfOnly
                };
                perHandler.Add(p);
                await perHandler.SaveChangesAsync();

                Assert.AreNotEqual(p.ID, 0);

                //2. Modify person info.
                Person beModifiedPerson = await perHandler.GetByIdAsync(p.ID);
                beModifiedPerson.RealName = "testGoodMan";
                beModifiedPerson.NickName = "testIAMGoodMan";
                beModifiedPerson.Gender = Gender.Male;
                beModifiedPerson.DOB = new DateTime(2015, 8, 21);
                beModifiedPerson.RealNameAccessLevel = PersonInfoAccessInfo.All;
                beModifiedPerson.DOBAccessLevel = PersonInfoAccessInfo.FollowerOnly;
                await perHandler.ModifyPersonInfoAsync(beModifiedPerson);

                //3. Test the modified person.
                Person modifiedPerson = await perHandler.GetByIdAsync(p.ID);
                Assert.IsNotNull(modifiedPerson);
                Assert.AreEqual(modifiedPerson.RealName, "testGoodMan");
                Assert.AreEqual(modifiedPerson.NickName, "testIAMGoodMan");
                Assert.AreEqual(modifiedPerson.Gender, Gender.Male);
                Assert.AreEqual(modifiedPerson.RealNameAccessLevel, PersonInfoAccessInfo.All);
                Assert.AreEqual(modifiedPerson.DOBAccessLevel, PersonInfoAccessInfo.FollowerOnly);
                Assert.AreEqual(modifiedPerson.HomePageAccessLevel, PersonInfoAccessInfo.MyselfOnly);
            }
        }

        [Test]
        public async Task Test_04_FollowAsync()
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                PersonHandler perHandler = new PersonHandler(dbContext);
                GroupHandler groupHandler = new GroupHandler(dbContext);
                GroupMemberHandler gmHandler = new GroupMemberHandler(dbContext);

                //1. test normal.
                Person sam = CreatePerson("TestSam", "TestSam");
                Person joan = CreatePerson("TestJoan", "TestJoan");

                bool isSucceed = await perHandler.FollowAsync(sam.ID, joan.ID);

                sam = await perHandler.Include(x => x.MyFollowingPersons, x => x.MyFans).SingleOrDefaultAsync(x => x.ID == sam.ID);
                joan = await perHandler.Include(x => x.MyFollowingPersons, x => x.MyFans).SingleOrDefaultAsync(x => x.ID == joan.ID);

                Assert.IsTrue(isSucceed);
                Assert.AreEqual(sam.MyFollowingPersons.Count, 1);
                Assert.AreEqual(sam.MyFans.Count, 0);
                Assert.AreEqual(joan.MyFollowingPersons.Count, 0);
                Assert.AreEqual(joan.MyFans.Count, 1);

                //2. test one more time follow, and check it count was changed.
                isSucceed = await perHandler.FollowAsync(sam.ID, joan.ID);

                sam = await perHandler.Include(x => x.MyFollowingPersons, x => x.MyFans).SingleOrDefaultAsync(x => x.ID == sam.ID);
                joan = await perHandler.Include(x => x.MyFollowingPersons, x => x.MyFans).SingleOrDefaultAsync(x => x.ID == joan.ID);

                Assert.IsTrue(isSucceed);
                Assert.AreEqual(sam.MyFollowingPersons.Count, 1);
                Assert.AreEqual(sam.MyFollowingPersons.First().FollowingID, joan.ID);
                Assert.AreEqual(sam.MyFans.Count, 0);
                Assert.AreEqual(joan.MyFollowingPersons.Count, 0);
                Assert.AreEqual(joan.MyFans.Count, 1);
                Assert.AreEqual(joan.MyFans.First().FollowerID, sam.ID);

                //3. test black list.
                Person jay = CreatePerson("TestJay", "TestJay");
                Person ken = CreatePerson("TestKen", "TestKen");

                Group blackGroup = await groupHandler.CreateGroupAsync(ken.ID, "TestBGroup", GroupType.BlackList);

                GroupMember gm_sam = new GroupMember()
                {
                    PersonID = jay.ID,
                    GroupID = blackGroup.ID
                };
                gmHandler.Add(gm_sam);
                gmHandler.SaveChanges();

                bool isChecked = false;
                try
                {
                    isSucceed = await perHandler.FollowAsync(jay.ID, ken.ID);
                }
                catch (Exception ex)
                {
                    isChecked = true;
                    Assert.AreEqual(ex.GetType(), typeof(DisplayableException));
                    Assert.AreEqual(ex.Message, "由于用户设置，你无法关注。");
                }
                Assert.IsTrue(isChecked);

                jay = await perHandler.Include(x => x.MyFollowingPersons, x => x.MyFans).SingleOrDefaultAsync(x => x.ID == jay.ID);
                ken = await perHandler.Include(x => x.MyFollowingPersons, x => x.MyFans).SingleOrDefaultAsync(x => x.ID == ken.ID);

                Assert.AreEqual(jay.MyFollowingPersons.Count, 0);
                Assert.AreEqual(jay.MyFans.Count, 0);
                Assert.AreEqual(ken.MyFollowingPersons.Count, 0);
                Assert.AreEqual(ken.MyFans.Count, 0);
            }
        }

        [Test]
        public async Task Test_05_UnFollowAsync()
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                PersonHandler perHandler = new PersonHandler(dbContext);

                //1. test normal and confirm follow succeed.

                Person kity = CreatePerson("TestKity", "TestKity");
                Person judy = CreatePerson("TestJudy", "TestJudy");

                bool isSucceed = await perHandler.FollowAsync(kity.ID, judy.ID);

                kity = await perHandler.Include(x => x.MyFollowingPersons, x => x.MyFans).SingleOrDefaultAsync(x => x.ID == kity.ID);
                judy = await perHandler.Include(x => x.MyFollowingPersons, x => x.MyFans).SingleOrDefaultAsync(x => x.ID == judy.ID);

                Assert.IsTrue(isSucceed);
                Assert.AreEqual(kity.MyFollowingPersons.Count, 1);
                Assert.AreEqual(kity.MyFans.Count, 0);
                Assert.AreEqual(judy.MyFollowingPersons.Count, 0);
                Assert.AreEqual(judy.MyFans.Count, 1);

                //2. unfollow now.
                bool isUnFollowSucceed = await perHandler.UnFollowAsync(kity.ID, judy.ID);

                kity = await perHandler.Include(x => x.MyFollowingPersons, x => x.MyFans).SingleOrDefaultAsync(x => x.ID == kity.ID);
                judy = await perHandler.Include(x => x.MyFollowingPersons, x => x.MyFans).SingleOrDefaultAsync(x => x.ID == judy.ID);

                Assert.IsTrue(isUnFollowSucceed);
                Assert.AreEqual(kity.MyFollowingPersons.Count, 0);
                Assert.AreEqual(kity.MyFans.Count, 0);
                Assert.AreEqual(judy.MyFollowingPersons.Count, 0);
                Assert.AreEqual(judy.MyFans.Count, 0);

                //3. unfollow again, it will be do nothing.
                isUnFollowSucceed = await perHandler.UnFollowAsync(kity.ID, judy.ID);

                Assert.IsTrue(isUnFollowSucceed);
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
    }
}
