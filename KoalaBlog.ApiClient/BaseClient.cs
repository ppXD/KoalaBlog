using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using KoalaBlog.Framework.Exceptions;
using KoalaBlog.ApiClient.Extensions;
using KoalaBlog.Security;

namespace KoalaBlog.ApiClient
{
    public class BaseClient
    {
        private const string UnknownError = "UnknownError";

        public BaseClient(Uri baseEndpoint)
        {
            BaseAddress = baseEndpoint;
        }

        protected async Task GetAsync(string requestUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                BuildHeaders(client);

                var response = await client.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfResponseNotSuccessful(response);
                }                                    
            }
        }

        protected async Task<T> GetAsync<T>(string requestUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                BuildHeaders(client);

                var response = await client.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    ThrowIfResponseNotSuccessful(response);
                }
                return default(T);
            }
        }

        protected async Task<TOutput> PostAsync<TOutput>(string requestUri, object data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                BuildHeaders(client);

                var response = await client.PostAsJsonAsync(requestUri, data);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<TOutput>();
                }
                else
                {
                    ThrowIfResponseNotSuccessful(response);
                }
                return default(TOutput);
            }
        }

        protected async Task<TOutput> PostFileAsync<TOutput>(string requestUri, HttpPostedFileBase file)
        {
            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                client.BaseAddress = BaseAddress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                BuildHeaders(client);

                content.Add(new StreamContent(file.InputStream), "fieldNameHere", file.FileName);

                var response = await client.PostAsync(requestUri, content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<TOutput>();
                }
                else
                {
                    ThrowIfResponseNotSuccessful(response);
                }
                return default(TOutput);
            }            
        }

        protected TOutput GetSync<TOutput>(string requestUri)
        {
            return SendSync<TOutput, object>(requestUri, HttpMethod.Get);
        }

        protected TOutput PostSync<TOutput, TInput>(string requestUri, TInput body) where TInput : class
        {
            return SendSync<TOutput, TInput>(requestUri, HttpMethod.Post, body);
        }

        private TOutput SendSync<TOutput, TInput>(string requestUri, HttpMethod httpMethod, TInput body = null) where TInput : class
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                BuildHeaders(client);

                Task<HttpResponseMessage> responseTask = null;

                if (httpMethod == HttpMethod.Get)
                {
                    responseTask = client.GetAsync(requestUri);
                }
                else if (httpMethod == HttpMethod.Post)
                {
                    responseTask = client.PostAsync(requestUri, body, new JsonMediaTypeFormatter());
                }

                responseTask.Wait();

                if (responseTask.Result.IsSuccessStatusCode)
                {
                    return responseTask.Result.Content.ReadAsAsync<TOutput>().Result;
                }
                else
                {
                    ThrowIfResponseNotSuccessful(responseTask.Result);
                }
                return default(TOutput);
            }
        }

        /// <summary>
        ///     Sends an http request.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        protected async Task SendAsync(Uri requestUri, HttpMethod httpMethod)
        {
            await SendAsync<object>(requestUri, httpMethod);
        }

        /// <summary>
        ///  Sends an http request.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        protected async Task<TOutput> SendAsync<TOutput>(Uri requestUri, HttpMethod httpMethod)
        {
            var requestMsg = new HttpRequestMessage(httpMethod, requestUri);

            return await SendAsync<TOutput>(requestMsg);
        }

        /// <summary>
        ///  Sends an http request.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="body">The body.</param>
        protected async Task SendAsync<TInput>(Uri requestUri, HttpMethod httpMethod, TInput body)
        {
            await SendAsync<TInput, object>(requestUri, httpMethod, body);
        }

        /// <summary>
        ///     Sends an http request.
        /// </summary>
        /// <typeparam name="TInput">Input type.</typeparam>
        /// <typeparam name="TOutput">Output type.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="body">The body.</param>
        /// <param name="userId">The user id. Only required by the tenant API.</param>
        protected async Task<TOutput> SendAsync<TInput, TOutput>(
            Uri requestUri,
            HttpMethod httpMethod,
            TInput body)
        {
            var message = new HttpRequestMessage(httpMethod, requestUri)
            {
                Content =
                    new ObjectContent<TInput>(
                        body,
                        CreateMediaTypeFormatter())
            };

            return await SendAsync<TOutput>(message);
        }

        private async Task<TOutput> SendAsync<TOutput>(HttpRequestMessage requestMsg)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                BuildHeaders(client);

                var response = await client.SendAsync(requestMsg);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<TOutput>();
                }
                else
                {
                    ThrowIfResponseNotSuccessful(response);
                }
                return default(TOutput);
            }
        }

        public static MediaTypeFormatter CreateMediaTypeFormatter()
        {
            //MediaTypeFormatter formatter;
            var formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings =
                {
                    DefaultValueHandling =
                        DefaultValueHandling.Ignore,
                    NullValueHandling =
                        NullValueHandling.Ignore//,
                    //TraceWriter = new DiagnosticsTraceWriter()
                }
            };

            return formatter;
        }

        private void BuildHeaders(HttpClient client)
        {
            try
            {
                //create credentials
                client.DefaultRequestHeaders.Authorization = CreateBearerCredentials(KoalaBlogSecurityManager.GetAuthCookie());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private AuthenticationHeaderValue CreateBearerCredentials(string accessToken)
        {
            return new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private void ThrowIfResponseNotSuccessful(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                //Do not treat not found as exception, but rather as null result
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException(response.ReasonPhrase, new Exception(response.RequestMessage.ToString()));
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    throw new ForbiddenException();
                }

                if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    string errorMsg = string.Empty;

                    try
                    {
                        errorMsg = response.Content.ReadAsStringAsync().Result ?? UnknownError;
                    }
                    catch (InvalidOperationException)
                    {
                        // ReadAsAsync will synchronously throw InvalidOperationException when there is no default formatter for the ContentType.
                        // We will treat these cases as an unknown error
                    }
                    catch (UnsupportedMediaTypeException) // can't parse the message, create it manually
                    {

                    }
                    
                    if(!string.IsNullOrEmpty(errorMsg))
                    {
                        throw new DisplayableException(errorMsg);
                    }
                }
            }
        }

        /// <summary>
        /// Creates the request URI.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="queryStringParameters">The query string parameters.</param>
        /// <returns>Request URI</returns>
        protected Uri CreateRequestUri(string relativePath, params KeyValuePair<string, string>[] queryStringParameters)
        {
            var queryString = string.Empty;

            if (queryStringParameters != null && queryStringParameters.Length > 0)
            {
                var queryStringProperties = HttpUtility.ParseQueryString(BaseAddress.Query);

                foreach (var queryStringParameter in queryStringParameters)
                {
                    queryStringProperties[queryStringParameter.Key] = queryStringParameter.Value;
                }

                queryString = queryStringProperties.ToString();
            }

            return CreateRequestUri(relativePath, queryString);
        }

        protected Uri CreateRequestUri(string relativePath, object queryStringParameters)
        {
            var parameters = queryStringParameters.ToPropertyDictionary()
                                                  .Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToNullOrString()))
                                                  .ToArray();
            return CreateRequestUri(relativePath, parameters);
        }

        /// <summary>
        /// Creates the request URI.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>Request URI</returns>
        protected Uri CreateRequestUri(string relativePath, string queryString)
        {
            var endpoint = new Uri(BaseAddress, relativePath);
            var uriBuilder = new UriBuilder(endpoint) { Query = queryString };
            return uriBuilder.Uri;
        }

        protected Uri BaseAddress { get; set; }
    }
}
