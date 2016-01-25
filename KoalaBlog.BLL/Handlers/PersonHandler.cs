using KoalaBlog.Entity.Models;
using KoalaBlog.Entity.Models.Enums;
using KoalaBlog.Framework.Exceptions;
using KoalaBlog.Framework.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class PersonHandler : PersonHandlerBase
    {
        private readonly DbContext _dbContext;

        public PersonHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 根据ID获取Person对象
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="paths">Include Path</param>
        /// <returns></returns>
        public async Task<Person> GetByIdAsync(long personId, params Expression<Func<Person, object>>[] paths)
        {
            return await this.Include(paths).SingleOrDefaultAsync(x => x.ID == personId);
        }

        /// <summary>
        /// 创建Person并且建立UserAccountXPerson的关系
        /// </summary>
        /// <param name="ua">UserAccount</param>
        /// <returns></returns>
        public async Task<Person> CreatePersonAsync(UserAccount ua)
        {
            AssertUtil.IsNotNull(ua, "UserAccount can't be null");

            UserAccountHandler uaHandler = new UserAccountHandler(_dbContext);
            UserAccountXPersonHandler uaxpHandler = new UserAccountXPersonHandler(_dbContext);

            AssertUtil.IsNotNull(await uaHandler.GetByIdAsync(ua.ID), "This user account doesn't exist");

            //1. Check whether the existing relationships.
            AssertUtil.IsTrue(await uaxpHandler.AnyAsync(x => x.UserAccountID == ua.ID), "Existing relationships");

            using(var dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    //2. Setup the basic profile.
                    Person per = new Person();
                    per.NickName = ua.UserName;
                    per.RealNameAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                    per.SexualTrendAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                    per.MaritalStatusAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                    per.QQAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                    per.DOBAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                    per.BloodTypeAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                    per.HomePageAccessLevel = PersonInfoAccessInfo.MyselfOnly;
                    per.AllowablePersonForComment = AllowablePersonForComment.All;
                    per.AllowCommentAttachContent = true;
                    Add(per);
                    await SaveChangesAsync();

                    UserAccountXPerson uaxp = new UserAccountXPerson();
                    uaxp.UserAccountID = ua.ID;
                    uaxp.PersonID = per.ID;
                    uaxpHandler.Add(uaxp);
                    await SaveChangesAsync();

                    dbTransaction.Commit();

                    return per;
                }
                catch (Exception)
                {
                    dbTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 修改Person的昵称
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="nickName">昵称</param>
        /// <returns></returns>
        public async Task<bool> ModifyPersonNickNameAsync(long personId, string nickName)
        {
            AssertUtil.Waterfall()
                .AreBigger(personId, 0, "personId must be greater than zero")
                .NotNullOrWhiteSpace(nickName, "nickName can't be empty")
                .Done();

            //1. Get the Person by personId.
            Person per = await GetByIdAsync(personId);

            AssertUtil.IsNotNull(per, "This person doesn't exist");

            //2. Modify person nick name.
            if(per != null)
            {
                per.NickName = nickName;

                MarkAsModified(per);

                return await SaveChangesAsync() > 0;
            }
            return false;
        }

        /// <summary>
        /// 修改Person个人资料
        /// </summary>
        /// <param name="person">Person对象</param>
        /// <returns></returns>
        public async Task<Person> ModifyPersonInfoAsync(Person person)
        {
            AssertUtil.IsNotNull(person, "person can't be null");

            MarkAsModified(person);

            await SaveChangesAsync();

            return person;
        }

        /// <summary>
        /// 根据UserAccountID获取Person
        /// </summary>
        /// <param name="userAccountID">用户ID</param>
        /// <returns></returns>
        public async Task<Person> LoadByUserAccountIDAsync(long userAccountID)
        {
            UserAccountXPersonHandler uaxpHandler = new UserAccountXPersonHandler(_dbContext);

            UserAccountXPerson uaxp = await uaxpHandler.LoadByUserAccountIDAsync(userAccountID);

            if(uaxp != null)
            {
                return uaxp.Person;
            }

            return null;
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="followerId">关注者ID</param>
        /// <param name="followingId">被关注者ID</param>
        /// <returns></returns>
        public async Task<bool> FollowAsync(long followerId, long followingId)
        {
            GroupHandler groupHandler = new GroupHandler(_dbContext);
            PersonXPersonHandler pxpHandler = new PersonXPersonHandler(_dbContext);

            //1. 检查被关注人黑名单是否存在关注人，若已拉黑，则无法关注。
            Group blackGroup = await groupHandler.Include(x => x.GroupMembers).SingleOrDefaultAsync(x => x.PersonID == followingId && x.Type == GroupType.BlackList);
            
            if(blackGroup != null && blackGroup.GroupMembers.Count > 0)
            {
                bool isInGroupMember = blackGroup.GroupMembers.Select(x => x.PersonID).Contains(followerId);

                if(isInGroupMember)
                {
                    throw new DisplayableException("由于用户设置，你无法关注。");
                }
            }

            //2. 检查关注人的关注名单是否已经关注了。
            bool isFollow = await pxpHandler.Fetch(x => x.FollowerID == followerId && x.FollowingID == followingId).SingleOrDefaultAsync() != null;

            //3. 如果未关注则添加纪录。
            if(!isFollow)
            {
                PersonXPerson pxp = new PersonXPerson()
                {
                    FollowerID = followerId,
                    FollowingID = followingId
                };
                pxpHandler.Add(pxp);

                return await SaveChangesAsync() > 0;
            }

            return true;
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="followerId">关注者ID</param>
        /// <param name="unFollowPerId">取消关注者ID</param>
        /// <returns></returns>
        public async Task<bool> UnFollowAsync(long followerId, long unFollowPerId)
        {
            PersonXPersonHandler pxpHandler = new PersonXPersonHandler(_dbContext);

            //1. 检查关注人的关注名单是否已经关注了。
            PersonXPerson pxp = await pxpHandler.Fetch(x => x.FollowerID == followerId && x.FollowingID == unFollowPerId).SingleOrDefaultAsync();

            //2. 如果存在记录，则删除记录，取消关注。
            if(pxp != null)
            {
                pxpHandler.MarkAsDeleted(pxp);

                return await pxpHandler.SaveChangesAsync() > 0;
            }

            return true;
        }
    }
}
