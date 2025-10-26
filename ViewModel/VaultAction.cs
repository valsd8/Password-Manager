using Newtonsoft.Json;
using Password_Manager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;

namespace Password_Manager.ViewModel
{
    public class VaultAction : INotifyPropertyChanged
    {

        private string _windowTitle = "uselessString that I need to clean up without breaking shit";
        private readonly SharedDataServices _sharedServices;
        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //boilerplate end
        
        

        public VaultAction()
        {
            _sharedServices = App.SharedData;
             Data data = _sharedServices.data;
            

        }
        
        
        public string SetWindowsTitle()
        {
            string filepath = System.IO.Path.Combine(Environment.CurrentDirectory, "encrypted.bin");
            
            string json = File.ReadAllText(filepath);
            EncryptedPackage encryptedPackage = JsonConvert.DeserializeObject<EncryptedPackage>(json);
            string name = $"Vault name: {encryptedPackage.Name}";
            
            return name;
        }
        public void addEntry(string username, string password, string optionalComment = "", string url = "")
        {
            try
            {
                var vault = App.SharedData.vault;

                vault.Entries.Add(new Data
                {
                    Email = username,
                    Password = password,
                    Url = url,
                    OptionalComment = optionalComment
                });
                string vaultJson = JsonConvert.SerializeObject(vault);
                string filepath = System.IO.Path.Combine(Environment.CurrentDirectory, "encrypted.bin");
                string fileVaultDataSerialized = File.ReadAllText(filepath);

                EncryptedPackage Data = JsonConvert.DeserializeObject<EncryptedPackage>(fileVaultDataSerialized);
                if (Data == null)
                {
                    MessageBox.Show("Failed to deserialize vault data.");
                    throw new Exception("Failed to deserialize vault data.");
                }
                    
                string base64iv = HandleFileEncryption.GenerateIv();

                byte[] iv = Convert.FromBase64String(base64iv);
                byte[] salt = Convert.FromBase64String(Data.Salt);
                var newEncryptedPackage = App.SharedData.EncryptedPackage;
                newEncryptedPackage.IV = base64iv;
                newEncryptedPackage.Salt = Data.Salt;
                newEncryptedPackage.Name = Data.Name;
                string encryptedVault = Convert.ToBase64String(DecryptBin.EncryptData(Encoding.UTF8.GetBytes(vaultJson), App.SharedData.aesKey, Convert.FromBase64String(newEncryptedPackage.IV)));
                newEncryptedPackage.EncryptedData = encryptedVault;

                string newEncryptedPackageSerialized = JsonConvert.SerializeObject(newEncryptedPackage);
                File.WriteAllText(filepath, newEncryptedPackageSerialized);
                DisplayData displayData = new DisplayData();
                displayData.displayData(View.VaultAction.StaticVaultListView);


            }
            catch(Exception ex)
            {
                


                MessageBox.Show(ex.ToString());
            }
            






        }
        public void DeleteEntry(string guid)
        {
            try
            {
                var vault = App.SharedData.vault;

                for (int i = 0; i < vault.Entries.Count; i++)
                {
                    if (vault.Entries[i].Id == guid)
                    {
                        var correspondingDataEntry = vault.Entries[i];
                        vault.Entries.RemoveAt(i);
                        break;
                    }
                   
                }
                string vaultJson = JsonConvert.SerializeObject(vault);
                string filepath = System.IO.Path.Combine(Environment.CurrentDirectory, "encrypted.bin");
                string fileVaultDataSerialized = File.ReadAllText(filepath);

                EncryptedPackage Data = JsonConvert.DeserializeObject<EncryptedPackage>(fileVaultDataSerialized);
                if (Data == null)
                {
                    MessageBox.Show("Failed to deserialize vault data.");
                    throw new Exception("Failed to deserialize vault data.");
                }

                string base64iv = HandleFileEncryption.GenerateIv();

                byte[] iv = Convert.FromBase64String(base64iv);
                byte[] salt = Convert.FromBase64String(Data.Salt);
                var newEncryptedPackage = App.SharedData.EncryptedPackage;
                newEncryptedPackage.IV = base64iv;
                newEncryptedPackage.Salt = Data.Salt;
                newEncryptedPackage.Name = Data.Name;
                string encryptedVault = Convert.ToBase64String(DecryptBin.EncryptData(Encoding.UTF8.GetBytes(vaultJson), App.SharedData.aesKey, Convert.FromBase64String(newEncryptedPackage.IV)));
                newEncryptedPackage.EncryptedData = encryptedVault;

                string newEncryptedPackageSerialized = JsonConvert.SerializeObject(newEncryptedPackage);
                File.WriteAllText(filepath, newEncryptedPackageSerialized);
                DisplayData displayData = new DisplayData();
                displayData.displayData(View.VaultAction.StaticVaultListView);







             
            }
            catch (Exception ex)
            {
                MessageBox.Show($"unhandled ex In vaultAction.deleteEntry: {ex.ToString()}");
            }

            

        }
        public string ValidateInput(string url, string username, string password, string optionalComment)
        {
            if (string.IsNullOrWhiteSpace(username))
                return "Username cannot be empty.";

            if (string.IsNullOrWhiteSpace(password))
                return "Password cannot be empty.";

           

            if (optionalComment?.Length > 2000)
                return "Comment is too long (max 2000 characters).";

            
            
            return null;
        }







    }
}
