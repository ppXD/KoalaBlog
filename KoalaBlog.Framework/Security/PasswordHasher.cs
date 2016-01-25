using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Framework.Security
{
    public class PasswordHasher
    {
        public static string HashPassword(string clearText)
        {
            return Crypto.HashPassword(clearText);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (Crypto.VerifyHashedPassword(hashedPassword, providedPassword))
            {
                return true;
            }
            return false;
        }

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            return Crypto.Encrypt(toEncrypt, useHashing);
        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            return Crypto.Decrypt(cipherString, useHashing);
        }
    }
}
