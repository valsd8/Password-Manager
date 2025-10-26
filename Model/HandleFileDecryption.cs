using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Password_Manager.Model
{
    public class HandleFileDecryption
    {
        public Vault DecryptFile(string filePath, string password)
        {
            string json = File.ReadAllText(filePath);
            EncryptedPackage encryptedPackage = JsonConvert.DeserializeObject<EncryptedPackage>(json);
            if (string.IsNullOrWhiteSpace(encryptedPackage?.EncryptedData))
            {
                return null; // Caller should handle this appropriately
            }
            byte[] iv = Convert.FromBase64String(encryptedPackage.IV);
            byte[] cipher = Convert.FromBase64String(encryptedPackage.EncryptedData);
            byte[] salt = Convert.FromBase64String(encryptedPackage.Salt);

            byte[] key = HandleFileEncryption.DeriveKey(password, salt);
            App.SharedData.aesKey = key;// Assume Encrypt is a static class handling key derivation

            byte[] decryptedBytes = DecryptBin.DecryptData(cipher, key, iv);
            if (decryptedBytes == null || decryptedBytes.Length == 0)
            {
                return null;
            }

            string plainJson = Encoding.UTF8.GetString(decryptedBytes);
            if (string.IsNullOrWhiteSpace(plainJson))
            {
                return null;
            }
            var decryptedData = App.SharedData.vault;
            decryptedData = JsonConvert.DeserializeObject<Vault>(plainJson);

            return decryptedData;
        }
    }
}
