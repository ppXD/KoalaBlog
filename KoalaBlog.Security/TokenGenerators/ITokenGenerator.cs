using System;

namespace KoalaBlog.Security.TokenGenerators
{
    public interface ITokenGenerator
    {
        string GenerateToken();
    }
}
