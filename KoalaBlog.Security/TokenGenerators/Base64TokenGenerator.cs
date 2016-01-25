using System;
using System.Text;

namespace KoalaBlog.Security.TokenGenerators
{
    public class Base64TokenGenerator : ITokenGenerator
    {
        public string GenerateToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", string.Empty).Replace("+", string.Empty);
        }
    }
}
