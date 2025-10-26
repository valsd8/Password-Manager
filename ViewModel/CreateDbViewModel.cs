using Newtonsoft.Json;
using Password_Manager.Model;
using Password_Manager.View;
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


namespace Password_Manager.ViewModel
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

            // Step 1: Create an empty but valid vault (Data object)
            var emptyVault = App.SharedData.vault;

            string plainJson = JsonConvert.SerializeObject(emptyVault);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainJson);

            // Step 2: Encrypt the data
            byte[] encryptedBytes = DecryptBin.EncryptData(plainBytes, key, iv); // <-- you need to write this!
            string base64Encrypted = Convert.ToBase64String(encryptedBytes);

            // Step 3: Create encrypted package
            var encryptedPackage = new EncryptedPackage
            {
                Name = name,
                Salt = base64Salt,
                IV = base64Iv,
                EncryptedData = base64Encrypted
            };

            // Step 4: Serialize and write to file
            string jsonObject = JsonConvert.SerializeObject(encryptedPackage);
            string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, "encrypted.bin");
            File.WriteAllText(filePath, jsonObject);



            var mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = mainWindow;


            return jsonObject;
        }

    }
}
