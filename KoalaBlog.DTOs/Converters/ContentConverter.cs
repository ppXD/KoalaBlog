using System;
using System.Linq;
using KoalaBlog.Entity.Models;
using KoalaBlog.Framework.Extensions;

namespace KoalaBlog.DTOs.Converters
{
    public static class ContentConverter
    {
        public static ContentDTO ToDTO(this Content entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var result = new ContentDTO() { ID = entity.ID, CreatedDate = entity.CreatedDate };

            result.Type = entity.Type.GetDescription();
            result.ContentBinary = entity.ContentBinary;
            result.MimeType = !string.IsNullOrEmpty(entity.MimeType) ? entity.MimeType : string.Empty;
            result.SeoFilename = !string.IsNullOrEmpty(entity.SeoFilename) ? entity.SeoFilename : string.Empty;
            result.ContentPath = !string.IsNullOrEmpty(entity.ContentPath) ? entity.ContentPath : string.Empty;            
            result.AltAttribute = !string.IsNullOrEmpty(entity.AltAttribute) ? entity.AltAttribute : string.Empty;
            result.TitleAttribute = !string.IsNullOrEmpty(entity.TitleAttribute) ? entity.TitleAttribute : string.Empty;
                        
            return result;
        }
    }
}
