using System;
using System.Linq;
using KoalaBlog.Entity.Models;
using KoalaBlog.Framework.Extensions;

namespace KoalaBlog.DTOs.Converters
{
    public static class PersonConverter
    {
        public static PersonDTO ToDTO(this Person entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var result = new PersonDTO() { ID = entity.ID };

            if (entity.Avatars.Count > 0)
            {
                Avatar activeAvatar = entity.Avatars.SingleOrDefault(x => x.IsActive);

                result.AvatarUrl = activeAvatar != null ? activeAvatar.AvatarPath : string.Empty;
            }

            result.DOB = entity.DOB;
            result.QQ = !string.IsNullOrEmpty(entity.QQ) ? entity.QQ : string.Empty;
            result.RealName = !string.IsNullOrEmpty(entity.RealName) ? entity.RealName : string.Empty;
            result.NickName = !string.IsNullOrEmpty(entity.NickName) ? entity.NickName : string.Empty;
            result.HomePage = !string.IsNullOrEmpty(entity.HomePage) ? entity.HomePage : string.Empty;
            result.AvatarUrl = !string.IsNullOrEmpty(result.AvatarUrl) ? result.AvatarUrl : string.Empty;
            result.Introduction = !string.IsNullOrEmpty(entity.Introduction) ? entity.Introduction : string.Empty;
            result.Gender = entity.Gender != null ? entity.Gender.GetDescription() : string.Empty;
            result.SexualTrend = entity.SexualTrend != null ? entity.SexualTrend.GetDescription() : string.Empty;
            result.MaritalStatus = entity.MaritalStatus != null ? entity.MaritalStatus.GetDescription() : string.Empty;
            result.BloodType = entity.BloodType != null ? entity.BloodType.GetDescription() : string.Empty;
            result.FollowingCount = entity.MyFollowingPersons.Count;
            result.FansCount = entity.MyFans.Count;
            result.BlogCount = entity.Blogs.Count(x => !x.IsDeleted);

            return result;
        }
    }
}
