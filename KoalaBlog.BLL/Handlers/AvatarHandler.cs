using KoalaBlog.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class AvatarHandler : AvatarHandlerBase
    {
        private readonly DbContext _dbContext;

        public AvatarHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取头像
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <returns></returns>
        public async Task<Avatar> GetActiveAvatarByPersonId(long personId)
        {
            return await Fetch(x => x.PersonID == personId && x.IsActive).SingleOrDefaultAsync();
        }

        /// <summary>
        /// 获取头像Url
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <returns></returns>
        public async Task<string> GetActiveAvatarUrlByPersonId(long personId)
        {
            var personAvatar = await GetActiveAvatarByPersonId(personId);

            return personAvatar.AvatarPath;
        }
            
        /// <summary>
        /// 生成默认头像
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <returns></returns>
        public async Task<Avatar> CreateDefaultAvatar(long personId)
        {
            Avatar avatar = new Avatar()
            {
                PersonID = personId,
                AvatarPath = "../Content/Images/avatar-default.jpg",
                MimeType = "jpg",
                IsActive = true
            };

            Add(avatar);

            await SaveChangesAsync();

            return avatar;
        }
    }
}
