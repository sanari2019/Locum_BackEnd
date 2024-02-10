using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Locum_Backend.Helpers
{
    public static class EncDscPassword
    {
        public static string secretKey = "12345678$#@$^@1ERF";

        public static string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return "";
            }
            else
            {
                password = password + secretKey;
                var passwordinBytes = Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(passwordinBytes);
            }
        }

        public static string? DecryptPassword(string encryptedPassword)
        {
            if (string.IsNullOrEmpty(encryptedPassword))
            {
                return null;
            }
            else
            {
                try
                {
                    var encodedBytes = Convert.FromBase64String(encryptedPassword);
                    var actualPassword = Encoding.UTF8.GetString(encodedBytes);
                    actualPassword = actualPassword.Substring(0, Math.Min(actualPassword.Length, secretKey.Length));
                    return actualPassword;
                }
                catch (FormatException ex)
                {
                    // Handle the FormatException (invalid Base64 string)
                    Console.WriteLine($"Error decoding Base64 string: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
