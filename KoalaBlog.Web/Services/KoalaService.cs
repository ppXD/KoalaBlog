using KoalaBlog.ApiClient;
using KoalaBlog.ApiClient.Extensions;
using System;
using System.Web;

namespace KoalaBlog.Web.Services
{
    public class KoalaService
    {
        private BlogClient _blogClient;
        private PersonClient _personClient;
        private CommentClient _commentClient;
        private ContentClient _contentClient;
        private SecurityClient _securityClient;

        private static readonly object _LockObject = new object();

        private const string cacheKey = "koalaservice";

        private KoalaService()
        {
            this._blogClient = ClientContext.Clients.CreateBlogClient();
            this._personClient = ClientContext.Clients.CreatePersonClient();
            this._commentClient = ClientContext.Clients.CreateCommentClient();
            this._contentClient = ClientContext.Clients.CreateContentClient();
            this._securityClient = ClientContext.Clients.CreateSecurityClient();
        }

        public static KoalaService Create()
        {
            var koalaService = HttpContext.Current.Cache[cacheKey] as KoalaService;

            if (koalaService != null) return koalaService;

            lock (_LockObject)
            {
                if (koalaService == null)
                {
                    koalaService = new KoalaService();
                    HttpRuntime.Cache.Insert(cacheKey, koalaService, null);
                }
            }

            return koalaService;
        }

        public SecurityClient SecurityClient
        {
            get
            {
                return this._securityClient ?? (this._securityClient = ClientContext.Clients.CreateSecurityClient());
            }
        }

        public PersonClient PersonClient
        {
            get
            {
                return this._personClient ?? (this._personClient = ClientContext.Clients.CreatePersonClient());
            }
        }

        public BlogClient BlogClient
        {
            get
            {
                return this._blogClient ?? (this._blogClient = ClientContext.Clients.CreateBlogClient());
            }
        }

        public CommentClient CommentClient
        {
            get
            {
                return this._commentClient ?? (this._commentClient = ClientContext.Clients.CreateCommentClient());
            }
        }

        public ContentClient ContentClient
        {
            get
            {
                return this._contentClient ?? (this._contentClient = ClientContext.Clients.CreateContentClient());
            }
        }
    }
}