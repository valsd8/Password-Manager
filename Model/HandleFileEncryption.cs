using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager.Model
{
    class HandleFileEncryption
    {
        public static string GenerateSalt()
        {
            byte[] buffer = new byte[32];
            RandomNumberGenerator.Fill(buffer); 
            return Convert.ToBase64String(buffer);
        }
        public static byte[] DeriveKey(string masterPassword, byte[] salt)
        {
           
            using var keyDerivation = new Rfc2898DeriveBytes(
                password: masterPassword,
                salt: salt,
                iterations: 100_000,
                hashAlgorithm: HashAlgorithmName.SHA256
            );

            byte[] aesKey = keyDerivation.GetBytes(32);
            return aesKey;
        }
        public static string GenerateIv()
        {
            byte[] iv;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateIV();
                iv = aesAlg.IV;
            }
            return Convert.ToBase64String(iv);
        }
    }





}
