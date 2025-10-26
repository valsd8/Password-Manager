using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager.Model
{
    public class DecryptBin
    {

        public static byte[] EncryptData(byte[] plainBytes, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                }
            }
        }
        public static byte[] DecryptData(byte[] cipher, byte[] key, byte[] iv)
        {
            byte[] decryptedBytes = null;

            // Set up the encryption objects
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Decrypt the input ciphertext using the AES algorithm
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    decryptedBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
                }
            }

            return decryptedBytes;
        }
    }
}
