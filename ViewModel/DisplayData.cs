using Newtonsoft.Json;
using PasswordManager;
using PasswordManager.Model;
using PasswordManager.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PasswordManager.ViewModel
{
    public class DisplayData
    {
        public void displayData(ListView targetListView)
        {
            string filepath = Path.Combine(Environment.CurrentDirectory, "encrypted.bin");
            string encryptedPackageJson = File.ReadAllText(filepath);

            EncryptedPackage encryptedPackage = JsonConvert.DeserializeObject<EncryptedPackage>(encryptedPackageJson);

            byte[] iv = Convert.FromBase64String(encryptedPackage.IV);
            byte[] encryptedDataBytes = Convert.FromBase64String(encryptedPackage.EncryptedData);

            byte[] decryptedBytes = DecryptBin.DecryptData(encryptedDataBytes, App.SharedData.aesKey, iv);

            string decryptedJson = Encoding.UTF8.GetString(decryptedBytes);

            Vault vault = JsonConvert.DeserializeObject<Vault>(decryptedJson);
            int i = -1;
            //foreach (Data entry in vault.Entries)
            {
                
                //i++;
                //MessageBox.Show($"Email: {entry.Email}, Password: {entry.Password}, Url: {entry.Url}, Optional Comment : {entry.OptionalComment}, Index: {i}");
                //Console.WriteLine($"Email: {entry.Email}, Password: {entry.Password}, Url: {entry.Url}, Index: {i}");
                
            }
            targetListView.ItemsSource = vault.Entries;


        }


    }
}
