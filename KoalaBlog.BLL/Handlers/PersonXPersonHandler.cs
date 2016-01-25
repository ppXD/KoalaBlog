using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class PersonXPersonHandler : PersonXPersonHandlerBase
    {
        private readonly DbContext _dbContext;

        public PersonXPersonHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 判断两个用户是否互相关注
        /// </summary>
        /// <param name="currentPersonId">当前用户ID</param>
        /// <param name="otherPersonId">其他用户ID</param>
        /// <returns></returns>
        public async Task<bool> IsFriendAsync(long followerId, long followingId)
        {
            //return await AnyAsync(x => x.FollowerID == currentPersonId && x.FollowingID == otherPersonId) && 
            //       await AnyAsync(x => x.FollowerID == otherPersonId && x.FollowingID == currentPersonId);

            //return await CountAsync(x => (x.FollowerID == followerId || x.FollowerID == followingId) && (x.FollowingID == followerId || x.FollowingID == followingId)) == 2;

            // best performance.
            return await Fetch(x => x.FollowerID == followerId && x.FollowingID == followingId).SingleOrDefaultAsync() != null &&
                   await Fetch(x => x.FollowerID == followingId && x.FollowingID == followerId).SingleOrDefaultAsync() != null;
        }

        /// <summary>
        /// 获取正在关注数量
        /// </summary>
        /// <param name="followerId"></param>
        /// <returns></returns>
        public async Task<int> GetFollowingCountAsync(long followerId)
        {
            return await CountAsync(x => x.FollowerID == followerId);
        }

        /// <summary>
        /// 获取粉丝数量
        /// </summary>
        /// <param name="followerId"></param>
        /// <returns></returns>
        public async Task<int> GetFansCountAsync(long followerId)
        {
            return await CountAsync(x => x.FollowingID == followerId);
        }
    }
}
