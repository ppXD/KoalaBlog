using KoalaBlog.Principal;
using KoalaBlog.WebApi.Core.Managers;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace KoalaBlog.WebApi.Filters
{
    public class KoalaBlogWebApiAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // if define allow Anonymous attribute, do nothing.
            if(SkipAuthentication(context.ActionContext))
            {
                return;
            }

            // 1. Look for credentials in the request.
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            // 2. If there are no credentials, set the error result.
            if (authorization == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            // 3. If there are credentials but the filter does not recognize the 
            //    authentication scheme, set the error result.
            if (authorization.Scheme != "Bearer")
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid authorization scheme", request);
                return;
            }

            // 4. If there are credentials that the filter understands, try to validate them.
            // 5. If the credentials are bad, set the error result.
            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            //string decodedCredentials = ExtractAuthorizationParameter(authorization.Parameter);

            //if (string.IsNullOrEmpty(decodedCredentials))
            //{
            //    context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
            //}

            IPrincipal principal = await new SecurityManager().AuthenticateBearerTokenAsync(authorization.Parameter);

            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);
            }
            else
            {
                context.Principal = principal;
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue("Bearer");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }

        public bool AllowMultiple
        {
            get { return false; }
        }

        private bool SkipAuthentication(HttpActionContext actionContext)
        {
            if(actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }

        private string ExtractAuthorizationParameter(string authorizationParameter)
        {
            byte[] credentialBytes;

            try
            {
                credentialBytes = Convert.FromBase64String(authorizationParameter);
            }
            catch (FormatException)
            {
                return null;
            }

            // The currently approved HTTP 1.1 specification says characters here are ISO-8859-1.
            // However, the current draft updated specification for HTTP 1.1 indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            Encoding encoding = Encoding.UTF8;
            // Make a writable copy of the encoding to enable setting a decoder fallback.
            encoding = (Encoding)encoding.Clone();
            // Fail on invalid bytes rather than silently replacing and continuing.
            encoding.DecoderFallback = DecoderFallback.ExceptionFallback;
            string decodedCredentials;

            try
            {
                decodedCredentials = encoding.GetString(credentialBytes);
            }
            catch (DecoderFallbackException)
            {
                return null;
            }

            if (String.IsNullOrEmpty(decodedCredentials))
            {
                return null;
            }

            var arr = decodedCredentials.Split(':');
            if (arr.Length < 2)
            {
                return null;
            }

            decodedCredentials = arr[0];
                        
            return decodedCredentials;
        }
    }

    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await Excute();
        }

        private async Task<HttpResponseMessage> Excute()
        {
            var actionDescriptor = Request.GetActionDescriptor();
            string controller = actionDescriptor.ControllerDescriptor.ControllerName;
            string action = actionDescriptor.ActionName;

            string sourceObj = String.Format("KoalaBlog.WebApi.Controllers.{0}Controller.{1}", controller, action);

            //async log
            Task logTask = new SecurityManager().WriteLogAsync(Entity.Models.Enums.LogLevel.CRITICAL, sourceObj, "Unauthorized");

            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            response.ReasonPhrase = ReasonPhrase;
            response.RequestMessage = Request;

            await logTask;

            return response;
        }

        public string ReasonPhrase { get; private set; }

        public HttpRequestMessage Request { get; private set; }
    }

    public class AddChallengeOnUnauthorizedResult : IHttpActionResult
    {
        public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
        {
            Challenge = challenge;
            InnerResult = innerResult;
        }

        public AuthenticationHeaderValue Challenge { get; private set; }

        public IHttpActionResult InnerResult { get; private set; }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await InnerResult.ExecuteAsync(cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Only add one challenge per authentication scheme.
                if (!response.Headers.WwwAuthenticate.Any((h) => h.Scheme == Challenge.Scheme))
                {
                    response.Headers.WwwAuthenticate.Add(Challenge);
                }
            }

            return response;
        }
    }
}