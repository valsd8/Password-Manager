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
        public async void DisplayBreachedPassword(string password, string website)
        {
            Console.WriteLine($"Checking: {new string('*', password.Length)}");

            int breachCount = await Pawned.CheckPasswordAsync(password);
            string message = GetPasswordStrengthMessage(breachCount, website, password);
            if(message.Contains("Really Strong"))
            {
                return;
            }
            
            Console.WriteLine(message);
            BreachWarningDialog.Show(message);
            Console.WriteLine();


        }
        public static string GetPasswordStrengthMessage(int breachCount, string website, string password)
        {
            string starPassword = new string('*', password.Length);
            if (website == null)
            {
                if (breachCount == 0)
                    return "Really Strong Password - Not found in any breaches!";
                else if (breachCount < 10)
                    return $"the password {starPassword} is a Weak Password - Found in {breachCount} breach(es). Consider changing it.";
                else if (breachCount < 100)
                    return $"the password {starPassword} is a Very Weak Password - Found in {breachCount} breaches. Change it immediately!";
                else
                    return $"the password {starPassword} is a Extremely Weak Password - Found in {breachCount:N0} breaches! This is a very common password! Please change it immediately!! ";

            }
            else
            {
                if (breachCount == 0)
                    return $"the password {starPassword} is a Really Strong Password - Not found in any breaches!";
                else if (breachCount < 10)
                    return $"the password {starPassword} is a Weak Password - Found in {breachCount} breach(es). Consider changing it on the website {website}.";
                else if (breachCount < 100)
                    return $"the password {starPassword} is a Very Weak Password - Found in {breachCount} breaches. Change immediately on the website {website}!";
                else
                    return $"the password {starPassword} is a Extremely Weak Password - Found in {breachCount:N0} breaches! This is a very common password! Please change it on the website {website} immediately!! ";
            }


        }


    }
}
