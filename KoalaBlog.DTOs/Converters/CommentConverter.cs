using KoalaBlog.Entity.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace KoalaBlog.DTOs.Converters
{
    public static class CommentConverter
    {
        public static CommentDTO ToDTO(this Comment entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var result = new CommentDTO() { ID = entity.ID, CreatedDate = entity.CreatedDate };

            result.ContentText = !string.IsNullOrEmpty(entity.Content) ? entity.Content : string.Empty;

            if(entity.Person != null)
            {
                result.Person = entity.Person.ToDTO();
            }

            if(entity.CommentXContents != null && entity.CommentXContents.Count > 0)
            {
                result.Contents = new List<ContentDTO>();

                foreach (var cxc in entity.CommentXContents)
                {
                    if(cxc.Content != null)
                    {
                        var content = cxc.Content.ToDTO();

                        result.Contents.Add(content);
                    }
                }
            }

            return result;
        }
    }
}
