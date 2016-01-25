using System;
using KoalaBlog.Framework.Common;

namespace KoalaBlog.ApiClient.Extensions
{
    public static class CommentClientExtension
    {
        public static CommentClient CreateCommentClient(this ApiClients source)
        {
            return CreateCommentClient(source, AppSettingConfig.BaseAddress);
        }

        public static CommentClient CreateCommentClient(this ApiClients source, string serviceUrl)
        {
            return new CommentClient(new Uri(serviceUrl));
        }
    }
}
