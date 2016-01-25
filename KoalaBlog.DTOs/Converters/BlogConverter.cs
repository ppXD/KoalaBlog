using System;
using System.Linq;
using System.Collections.Generic;
using KoalaBlog.Entity.Models;
using KoalaBlog.Framework.Extensions;

namespace KoalaBlog.DTOs.Converters
{
    public static class BlogConverter
    {
        public static BlogDTO ToDTO(this Blog entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var result = new BlogDTO() { ID = entity.ID, CreatedDate = entity.CreatedDate };

            result.ContentText = !string.IsNullOrEmpty(entity.Content) ? entity.Content : string.Empty;
            result.CommentCount = entity.Comments.Count;
            result.RepostCount = entity.BaseBlogXBlogs.Count;

            if(entity.Person != null)
            {
                result.Person = entity.Person.ToDTO();
            }

            if(entity.BlogXContents != null && entity.BlogXContents.Count > 0)
            {
                result.Contents = new List<ContentDTO>();

                foreach (var bxc in entity.BlogXContents)
                {
                    if(bxc.Content != null)
                    {                        
                        var content = bxc.Content.ToDTO();

                        result.Contents.Add(content);
                    }
                }
            }
            
            return result;
        }
    }
}
