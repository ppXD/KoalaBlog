using KoalaBlog.Entity.Models;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class FavoriteHandler : FavoriteHandlerBase
    {
        private readonly DbContext _dbContext;

        public FavoriteHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取Blog收藏的数量
        /// </summary>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<int> GetFavoriteCountAsync(long blogId)
        {
            return await CountAsync(x => x.BlogID == blogId);
        }

        /// <summary>
        /// 判断用户是否收藏了Blog
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<bool> IsFavoriteAsync(long personId, long blogId)
        {
            return await Fetch(x => x.PersonID == personId && x.BlogID == blogId).SingleOrDefaultAsync() != null;
        }

        /// <summary>
        /// 收藏或者取消收藏
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<bool> CollectAsync(long personId, long blogId)
        {
            bool isFavorite = false;

            //1. 获取EntityConcurruentLock对象，用于处理Concurrent。
            EntityConcurrentLockHandler entityConcurrentLockHandler = new EntityConcurrentLockHandler(_dbContext);

            var entityLikeLockObj = await entityConcurrentLockHandler.GetByIdAsync(personId);

            if (entityLikeLockObj == null)
            {
                EntityConcurrentLock entityLikeLock = new EntityConcurrentLock() { PersonID = personId };

                entityConcurrentLockHandler.Add(entityLikeLock);
            }
            else
            {
                entityConcurrentLockHandler.MarkAsModified(entityLikeLockObj);
            }

            //2. 获取Favorite对象。
            var favoriteObj = await Fetch(x => x.PersonID == personId && x.BlogID == blogId).SingleOrDefaultAsync();

            //3. 如果Favorite不存在则为收藏，存在则取消收藏。
            if(favoriteObj == null)
            {
                favoriteObj = new Favorite()
                {
                    PersonID = personId,
                    BlogID = blogId
                };
                Add(favoriteObj);

                isFavorite = true;
            }
            else
            {
                MarkAsDeleted(favoriteObj);

                isFavorite = false;
            }

            await _dbContext.SaveChangesAsync();

            return isFavorite;
        }
    }
}
