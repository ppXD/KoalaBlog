using System;
using KoalaBlog.Framework.Common;

namespace KoalaBlog.ApiClient.Extensions
{
    public static class BlogClientExtension
    {
        public static BlogClient CreateBlogClient(this ApiClients source)
        {
            return (CreateBlogClient(source, AppSettingConfig.BaseAddress));
        }

        public static BlogClient CreateBlogClient(this ApiClients source, string serviceUrl)
        {
            return new BlogClient(new Uri(serviceUrl));
        }
    }
}
