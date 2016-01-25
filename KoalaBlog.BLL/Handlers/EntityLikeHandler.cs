using KoalaBlog.Entity.Models;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class EntityLikeHandler : EntityLikeHandlerBase
    {
        private readonly DbContext _dbContext;

        public EntityLikeHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 赞或者取消赞
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="entityId">EntityID</param>
        /// <param name="entityTable">EntityTable</param>
        /// <returns></returns>
        public async Task<Tuple<bool, int>> LikeAsync(long personId, long entityId, Type entityTable)
        {
            bool isLike = false;

            //1. 获取EntityConcurruentLock对象，用于处理Concurrent。
            EntityConcurrentLockHandler entityConcurrentLockHandler = new EntityConcurrentLockHandler(_dbContext);

            var entityLikeLockObj = await entityConcurrentLockHandler.GetByIdAsync(personId);

            if(entityLikeLockObj == null)
            {
                EntityConcurrentLock entityLikeLock = new EntityConcurrentLock() { PersonID = personId };

                entityConcurrentLockHandler.Add(entityLikeLock);
            }
            else
            {
                entityConcurrentLockHandler.MarkAsModified(entityLikeLockObj);
            }

            //2. 获取EntityLike对象。
            var entityLikeObj = await Fetch(x => x.PersonID == personId && x.EntityID == entityId && x.EntityTableName == entityTable.Name).SingleOrDefaultAsync();

            //3. 如果不存在则为点赞，存在则取消赞。
            if(entityLikeObj == null)
            {
                entityLikeObj = new EntityLike()
                {
                    PersonID = personId,
                    EntityID = entityId,
                    EntityTableName = entityTable.Name
                };
                Add(entityLikeObj);

                isLike = true;
            }
            else
            {
                MarkAsDeleted(entityLikeObj);

                isLike = false;
            }

            await _dbContext.SaveChangesAsync();

            //4. 获取此点赞的Entity的总赞数。
            int entityLikeCount = await CountAsync(x => x.EntityID == entityId && x.EntityTableName == entityTable.Name);

            return new Tuple<bool, int>(isLike, entityLikeCount);
        }

        /// <summary>
        /// 获取Blog点赞的数量
        /// </summary>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<int> GetBlogLikeCountAsync(long blogId)
        {
            return await CountAsync(x => x.EntityID == blogId && x.EntityTableName == typeof(Blog).Name);
        }

        /// <summary>
        /// 获取评论点赞的数量
        /// </summary>
        /// <param name="commentId">CommentID</param>
        /// <returns></returns>
        public async Task<int> GetCommentLikeCountAsync(long commentId)
        {
            return await CountAsync(x => x.EntityID == commentId && x.EntityTableName == typeof(Comment).Name);
        }

        /// <summary>
        /// 判断用户是否点赞了
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<bool> IsLikeAsync(long personId, long entityId, Type entityType)
        {
            return await Fetch(x => x.EntityID == entityId && x.EntityTableName == entityType.Name && x.PersonID == personId).FirstOrDefaultAsync() != null;
        }
    }
}
