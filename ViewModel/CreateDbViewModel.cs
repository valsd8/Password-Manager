using Newtonsoft.Json;
using PasswordManager.View;
using PasswordManager.Model;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Shapes;


namespace PasswordManager.ViewModel
{
    public class CreateDbViewModel : INotifyPropertyChanged

    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        //boilerplate end
        public ICommand CreateNewVaultBtn_Click { get; }
        private string _myText;

        public CreateDbViewModel()
        {
            
            
        }
        public static string CreateNewVault(string name, string password)
        {
            password = password?.Trim();
            string base64Salt = HandleFileEncryption.GenerateSalt();
            byte[] salt = Convert.FromBase64String(base64Salt);
            string base64Iv = HandleFileEncryption.GenerateIv();
            byte[] iv = Convert.FromBase64String(base64Iv);
            byte[] key = HandleFileEncryption.DeriveKey(password, salt);
            App.SharedData.aesKey = key;

           
            var emptyVault = App.SharedData.vault;

            string plainJson = JsonConvert.SerializeObject(emptyVault);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainJson);

            
            byte[] encryptedBytes = DecryptBin.EncryptData(plainBytes, key, iv);
            string base64Encrypted = Convert.ToBase64String(encryptedBytes);

            var encryptedPackage = new EncryptedPackage
            {
                Name = name,
                Salt = base64Salt,
                IV = base64Iv,
                EncryptedData = base64Encrypted
            };

           
            string jsonObject = JsonConvert.SerializeObject(encryptedPackage);
            string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, "encrypted.bin");
            File.WriteAllText(filePath, jsonObject);



            var mainWindow = new PasswordManager.View.MainWindow();
            mainWindow.Show();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = mainWindow;


            return jsonObject;
        }

    }
}
