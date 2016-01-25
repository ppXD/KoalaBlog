using KoalaBlog.WebApi.Core.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace KoalaBlog.WebApi.Controllers
{
    [RoutePrefix("koala/api/content")]
    public class ContentController : BaseApiController
    {        
        [Route("UploadImage"), HttpPost]
        public async Task<HttpResponseMessage> UploadImageAsync()
        {
            //1. Check if the request contains multipart/form-data.
            if(!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //2. Prepare ImageOnlyMultipartMemoryStreamProvider in which our multipart form. data will be loaded.            
            var rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images");
            var provider = new ImageOnlyMultipartMemoryStreamProvider(rootPath);

            try
            {
                //3. Read all contents of multipart message into ImageOnlyMultipartMemoryStreamProvider.
                await Request.Content.ReadAsMultipartAsync(provider);

                List<Tuple<string, string>> fileData = new List<Tuple<string, string>>();

                foreach (MultipartFileData file in provider.FileData)
                {
                    string mimeType = Path.GetExtension(file.LocalFileName);
                    string contentPath = "http://" + Request.RequestUri.Authority + "/Images/" + Path.GetFileName(file.LocalFileName);

                    fileData.Add(new Tuple<string, string>(mimeType, contentPath));
                }

                //4. Create pictures Content Entity.
                if (fileData.Count > 0)
                {
                    var result = await new ContentManager().CreateImagesContentAsync(fileData);

                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }         
        }

        [Route("DeleteImage/{contentId:long}"), HttpGet]
        public async Task<IHttpActionResult> DeleteImageAsync(long contentId)
        {
            var filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images");

            await new ContentManager().DeleteSingleImageContentAsync(contentId, filePath);

            return Ok();
        }

        [Route("GetOwnsContents/{personId:long}/{pageIndex:int=1}/{pageSize:int=10}"), HttpGet]
        public async Task<IHttpActionResult> GetOwnsContents(long personId, int pageIndex, int pageSize)
        {
            var result = await new ContentManager().GetOwnContents(personId, pageIndex, pageSize);

            return Ok(result);
        }

    }

    // We implement MultipartFormDataStreamProvider to override the filename of File which
    // will be stored on server, or else the default name will be of the format like Body-
    // Part_{GUID}. In the following implementation we simply get the FileName from 
    // ContentDisposition Header of the Request Body.
    public class ImageOnlyMultipartMemoryStreamProvider  : MultipartFormDataStreamProvider
    {
        private readonly string[] _allowedExtension = { ".jpg", ".jpeg", ".png", ".gif" };

        public ImageOnlyMultipartMemoryStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return Path.GetRandomFileName() + Path.GetExtension(headers.ContentDisposition.FileName.Replace("\"", string.Empty));
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            var fileExtension = Path.GetExtension(headers.ContentDisposition.FileName.Replace("\"", string.Empty));

            return _allowedExtension == null || _allowedExtension.Any(x => x.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)) ? base.GetStream(parent, headers) : Stream.Null;
        }
    }
}
