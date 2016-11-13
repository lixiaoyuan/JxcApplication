using System;
using System.Security.Cryptography;
using System.Text;

namespace Utilities
{

    public class PasswordHelper
    {

        public static bool Encrypt(ref string password,ref string salt)
        {
            try
            {
                if (string.IsNullOrEmpty(salt))
                {
                    salt = Guid.NewGuid().ToString();
                }
                byte[] passwordAndSaltBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = new SHA256Managed().ComputeHash(passwordAndSaltBytes);

                password = Convert.ToBase64String(hashBytes);
                return true;
            }
            catch (Exception )
            {
                return false;
            }

        }
    }
}
