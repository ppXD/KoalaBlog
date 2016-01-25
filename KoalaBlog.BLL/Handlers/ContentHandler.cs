using KoalaBlog.Entity.Models;
using KoalaBlog.Framework.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class ContentHandler : ContentHandlerBase
    {
        private readonly DbContext _dbContext;

        public ContentHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 创建类型为图片的Content
        /// </summary>
        /// <param name="contentPath">路径</param>
        /// <param name="mimeType">文件类型</param>
        /// <param name="contentBinary">文件二进制</param>
        /// <param name="altAttribute">Alt属性</param>
        /// <param name="titleAttribute">标题属性</param>
        /// <param name="seoFileName">Seo</param>
        /// <returns></returns>
        public async Task<List<Content>> CreateImagesContentAsync(List<Tuple<string, string>> imageInfos)
        {
            List<Content> contents = new List<Content>();

            foreach (var imageInfo in imageInfos)
            {
                Content content = new Content()
                {
                    MimeType = imageInfo.Item1,
                    ContentPath = imageInfo.Item2,
                    Type = Entity.Models.Enums.ContentType.Photo
                };
                contents.Add(content);
            }

            Add(contents);

            return await SaveChangesAsync() > 0 ? contents : null;
        }
    }
}
