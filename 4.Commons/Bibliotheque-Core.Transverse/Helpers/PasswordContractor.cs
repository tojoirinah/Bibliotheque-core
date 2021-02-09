using System;
using System.Security.Cryptography;

namespace Bibliotheque.Transverse.Helpers
{
    public static class PasswordContractor
    {
        public static string GeneratePassword(string password, string salt)
        {
            byte[] HexAlphabet = new byte[16] {
                    0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
                    0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66
                };

            string ByteArrayToHexString(byte[] value)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                int length = value.Length;

                if (length == 0)
                {
                    return string.Empty;
                }

                char[] output = new char[length << 1];

                for (int i = 0; i < value.Length; i++)
                {
                    output[i << 1] = (char)HexAlphabet[value[i] >> 4];
                    output[(i << 1) + 1] = (char)HexAlphabet[value[i] & 0x0F];
                }

                return new string(output);
            }

            string GenerateSHA256Hash(string input, string salt)
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
                var sha256hashString = new SHA256Managed();
                byte[] hash = sha256hashString.ComputeHash(bytes);
                return ByteArrayToHexString(hash);
            }

            return GenerateSHA256Hash(password, salt);
        }

        public static string CreateRandomPassword(int length = 15)
        {
            // Create a string of characters, numbers, special characters that allowed in the password
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }


        //public static string CreateSalt(int size)
        //{
        //    var rng = new RNGCryptoServiceProvider();
        //    var buff = new byte[size];
        //    rng.GetBytes(buff);
        //    return Convert.ToBase64String(buff);
        //}

    }
}
