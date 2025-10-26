using Newtonsoft.Json;
using Password_Manager.Model;
using Password_Manager.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;




namespace Password_Manager.ViewModel
{


    public class EnterPasswordViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //end of boilerplate
        public ICommand SubmitPasswordCommand { get; }
        private string _myText;
        private SharedDataServices _sharedServices;


        public string MyText
        {
            get => _myText;
            set
            {
                _myText = value;
                OnPropertyChanged();
            }
        }
        public readonly Action<object> _navigate;
        public EnterPasswordViewModel(Action<object> navigate)
        {
            SubmitPasswordCommand = new RelayCommand(SubmitPassword);
            
            _navigate = navigate;
            _sharedServices = App.SharedData;



        }

        private void SubmitPassword(object parameter)
           {
            
            if(parameter is PasswordBox passwordInput)
            {
                string password  = passwordInput.Password;
                string clearTextPasswd = password?.Trim();
                _sharedServices.password = clearTextPasswd;
                MyText = $"Decrypting ... password is {clearTextPasswd}";
                try
                {
                    var fileDecryptor = new HandleFileDecryption();
                    string filepath = Path.Combine(Environment.CurrentDirectory, "encrypted.bin");
                    if (!File.Exists(filepath))
                    {
                        MyText = "Encrypted file not found.";
                        return;
                    }

                    byte[] fileBytes = File.ReadAllBytes(filepath);
                    if (fileBytes.Length == 0)
                    {
                        MyText = "Encrypted file is empty.";
                        return;
                    }

                    try
                    {
                        var decryptedData = fileDecryptor.DecryptFile(filepath, clearTextPasswd);

                        //if (decryptedData == null)
                        {
                            //MyText = "Password is incorrect or decryption failed.";
                            //return;
                        }

                        // Optional: You can check if the vault has any entries
                        if (decryptedData.Entries == null || decryptedData.Entries.Any(e => string.IsNullOrEmpty(e.Password)))
                        {
                            MyText = "Vault is empty. No credentials saved yet.";
                            // You could still allow navigation if you want:
                             _navigate?.Invoke(new VaultAction());
                            return;
                        }
                        _sharedServices.password = clearTextPasswd;
                        _sharedServices.vault = decryptedData;
                        _navigate?.Invoke(new VaultAction());
                        DisplayData displayData = new DisplayData();
                        displayData.displayData(View.VaultAction.StaticVaultListView);
                        
                        
                    }
                    catch (System.NullReferenceException)
                    {

                    }
                    catch (Exception ex)
                    {
                        MyText = $"Wrong password or vault corrupted ";
                    }
                    
                    









                    //MyText = $"decrypted data: url : {decryptedData.Url}, email: {decryptedData.Email}";

                }
                catch (JsonException jsonEx)
                {
                    MessageBox.Show(jsonEx.ToString());
                    
                    return;
                }
                catch (IOException IOex)
                {
                    MessageBox.Show($"Error writing /opening / reading file data: {IOex.ToString()}");
                    return;
                }
                catch (System.NullReferenceException nullRefExceptionex)
                {
                    MessageBox.Show($"fucking null reference exception boy !! {nullRefExceptionex.ToString()}");
                    //MessageBox.Show($"Null ref Exception catched: {nullRefExceptionex.ToString()}");
                    //MyText = $"Null ref Exception catched: {nullRefExceptionex.Message}";
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    MyText = $"unhandled error: {ex.ToString()}";
                    return;
                }
                


            }
            else
            {
                MyText = "wrong parameter";
            }
         }
    }

}
