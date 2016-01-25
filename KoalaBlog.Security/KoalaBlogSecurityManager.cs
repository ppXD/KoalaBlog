using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace KoalaBlog.Security
{
    public class KoalaBlogSecurityManager
    {
        /// <summary>
        /// Create salt key
        /// </summary>
        /// <param name="size">Key size</param>
        /// <returns>Salt key</returns>
        public static string CreateSaltKey(int size)
        {
            // Generate a cryptographic random number
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }

        /// <summary>
        /// Create a password hash
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="saltkey">Salk key</param>
        /// <param name="passwordFormat">Password format (hash algorithm)</param>
        /// <returns>Password hash</returns>
        public static string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1")
        {
            if (String.IsNullOrEmpty(passwordFormat))
                passwordFormat = "SHA1";
            string saltAndPassword = String.Concat(password, saltkey);

            //return FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, passwordFormat);
            var algorithm = HashAlgorithm.Create(passwordFormat);
            if (algorithm == null)
                throw new ArgumentException("Unrecognized hash name");

            var hashByteArray = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }

        /// <summary>
        /// Set auth cookie
        /// </summary>
        /// <param name="accessToken">access token</param>
        public static void SetAuthCookie(string accessToken)
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies["KOALA.AUTH"] ?? new HttpCookie("KOALA.AUTH");
            authCookie.Value = accessToken;
            authCookie.Expires = DateTime.MaxValue;
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        /// <summary>
        /// Get auth cookie
        /// </summary>
        /// <returns></returns>
        public static string GetAuthCookie()
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies["KOALA.AUTH"];

            if(authCookie != null)
            {
                return authCookie.Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// Remove auth cookie
        /// </summary>
        public static void RemoveAuthCookie()
        {
            if(HttpContext.Current.Request.Cookies["KOALA.AUTH"] != null)
            {
                HttpCookie authCookie = new HttpCookie("KOALA.AUTH");
                authCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }
    }
}
