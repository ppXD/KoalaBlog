using System.Web.Http;
using KoalaBlog.WebApi.Filters;
using FluentValidation;
using FluentValidation.WebApi;

namespace KoalaBlog.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new KoalaBlogWebApiAuthorizeAttribute());
            config.Filters.Add(new KoalaBlogWebApiAuthenticationAttribute());
            config.Filters.Add(new KoalaBlogWebApiExceptionHandlerAttribute());
            config.Filters.Add(new KoalaBlogWebApiValidateModelAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Configure FluentValidation model validator provider
            FluentValidationModelValidatorProvider.Configure(config);
        }
    }
}
