using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using KoalaBlog.DAL;
using KoalaBlog.DTOs;
using KoalaBlog.DTOs.Converters;
using KoalaBlog.BLL.Handlers;

using KoalaBlog.Entity.Models;

namespace KoalaBlog.WebApi.Core.Managers
{
    public class PersonManager : ManagerBase
    {
        public async Task<string> GetPersonNickNameByUserAccountIDAsync(long userAccountID)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                PersonHandler perHandler = new PersonHandler(dbContext);
                UserAccountXPersonHandler uaxpHandler = new UserAccountXPersonHandler(dbContext);

                //1. Get the UserAccountXPerson
                UserAccountXPerson uaxp = await uaxpHandler.LoadByUserAccountIDAsync(userAccountID);

                //2. Get the Person NickName
                return uaxp != null && uaxp.Person != null ? uaxp.Person.NickName : "";
            }
        }

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <returns></returns>
        public async Task<PersonDTO> GetPersonInfoAsync(long personId)
        {
            //1. 获取Person的Task。
            Task<Person> personTask = GetPersonByIdAsync(personId);

            //2. 获取关注数量的Task。
            Task<int> followingCountTask = GetFollowingCountAsync(personId);

            //3. 获取粉丝数量的Task。
            Task<int> fansCountTask = GetFansCountAsync(personId);

            //4. 获取Blog数量的Task。
            Task<int> blogCountTask = GetBlogCountAsync(personId);

            //5. 获取头像Url的Task。
            Task<string> avatarUrlTask = GetActiveAvatarUrlAsync(personId);

            //6. 构造DTO对象。
            PersonDTO retVal = (await personTask).ToDTO();

            retVal.FollowingCount = await followingCountTask;
            retVal.FansCount = await fansCountTask;
            retVal.BlogCount = await blogCountTask;
            retVal.AvatarUrl = await avatarUrlTask;

            return retVal;
        }

        /// <summary>
        /// 根据PersonID获取Person
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <returns></returns>
        public async Task<Person> GetPersonByIdAsync(long personId)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                PersonHandler perHandler = new PersonHandler(dbContext);

                return await perHandler.GetByIdAsync(personId);                
            }
        }

        /// <summary>
        /// 获取正在关注数量
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <returns></returns>
        public async Task<int> GetFollowingCountAsync(long personId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                PersonXPersonHandler pxpHandler = new PersonXPersonHandler(dbContext);

                return await pxpHandler.GetFollowingCountAsync(personId);
            }
        }

        /// <summary>
        /// 获取粉丝数量
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <returns></returns>
        public async Task<int> GetFansCountAsync(long personId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                PersonXPersonHandler pxpHandler = new PersonXPersonHandler(dbContext);

                return await pxpHandler.GetFansCountAsync(personId);
            }
        }

        /// <summary>
        /// 获取发表的Blog数量
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <returns></returns>
        public async Task<int> GetBlogCountAsync(long personId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler bHandler = new BlogHandler(dbContext);

                return await bHandler.GetBlogCountAsync(personId);
            }
        }

        /// <summary>
        /// 获取头像的路径
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <returns></returns>
        public async Task<string> GetActiveAvatarUrlAsync(long personId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                AvatarHandler avatarHandler = new AvatarHandler(dbContext);

                var activeAvatar = await avatarHandler.GetActiveAvatarByPersonId(personId);

                return activeAvatar.AvatarPath;
            }
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="followerId">关注者ID</param>
        /// <param name="followingId">被关注者ID</param>
        /// <returns></returns>
        public async Task<bool> FollowAsync(long followerId, long followingId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                PersonHandler perHandler = new PersonHandler(dbContext);

                return await perHandler.FollowAsync(followerId, followingId);
            }
        }
    }
}