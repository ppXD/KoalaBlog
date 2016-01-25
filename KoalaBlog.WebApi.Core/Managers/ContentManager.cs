using KoalaBlog.BLL.Handlers;
using KoalaBlog.DAL;
using KoalaBlog.DTOs;
using KoalaBlog.DTOs.Converters;
using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace KoalaBlog.WebApi.Core.Managers
{
    public class ContentManager : ManagerBase
    {
        /// <summary>
        /// 创建图片Content
        /// </summary>
        /// <param name="contentPath">图片路径</param>
        /// <param name="mimeType">图片类型</param>
        /// <returns></returns>
        public async Task<List<ContentDTO>> CreateImagesContentAsync(List<Tuple<string, string>> imageInfos)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                ContentHandler contentHandler = new ContentHandler(dbContext);

                //1. 根据ImageInfos集合创建Content对象。
                var contents = await contentHandler.CreateImagesContentAsync(imageInfos);

                List<ContentDTO> contentDTOs = new List<ContentDTO>();

                if(contents != null && contents.Count > 0)
                {                    
                    //2. 生成DTO对象。
                    foreach (var content in contents)
                    {
                        contentDTOs.Add(content.ToDTO());
                    }
                }

                return contentDTOs;
            }
        }

        /// <summary>
        /// 删除单张图片
        /// </summary>
        /// <param name="contentId">ContentID</param>
        /// <returns></returns>
        public async Task DeleteSingleImageContentAsync(long contentId, string filePath)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                using(var dbTransaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        ContentHandler contentHandler = new ContentHandler(dbContext);

                        //1. 根据ContentID获取Content对象。
                        var content = await contentHandler.GetByIdAsync(contentId);

                        if(content != null)
                        {
                            FileInfo fileInfo = new FileInfo(filePath + "\\" + Path.GetFileName(content.ContentPath));
                            
                            //2. 判断当前Content的图片路径是否存在图片，存在则删除图片。
                            if(fileInfo.Exists)
                            {
                                var deleteTask = Task.Factory.StartNew(() =>
                                                {
                                                    fileInfo.Delete();
                                                });

                                contentHandler.MarkAsDeleted(content);

                                await contentHandler.SaveChangesAsync();
                                await deleteTask;

                                //3. 刷新FileInfo对象。
                                fileInfo.Refresh();

                                //4. 如果图片依旧存在证明删除失败，回滚事务。
                                if(fileInfo.Exists)
                                {
                                    dbTransaction.Rollback();
                                }
                            }                     
                        }
                        dbTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbTransaction.Rollback();                        
                    }
                }
            }
        }

        /// <summary>
        /// 获取用户自己的Contents
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public async Task<Tuple<bool, List<ContentDTO>>> GetOwnContents(long personId, int pageIndex = 1, int pageSize = int.MaxValue)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                List<ContentDTO> contentDtoList = null;

                BlogXContentHandler bxcHandler = new BlogXContentHandler(dbContext);

                //1. 获取Contents集合。
                var contentsTuple = await bxcHandler.GetContentsByPersonIdAsync(personId, pageIndex, pageSize);

                var contents = contentsTuple.Item2;

                if(contents != null && contents.Count > 0)
                {
                    contentDtoList = new List<ContentDTO>();

                    //2. 遍历生成DTO对象。
                    foreach (var content in contents)
                    {
                        contentDtoList.Add(content.ToDTO());
                    }
                }

                return new Tuple<bool,List<ContentDTO>>(contentsTuple.Item1, contentDtoList);
            }
        }

    }
}
