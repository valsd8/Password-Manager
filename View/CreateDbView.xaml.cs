using Password_Manager.Model;
using Password_Manager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;

namespace Password_Manager.View
{
    /// <summary>
    /// Logique d'interaction pour CreateDbView.xaml
    /// </summary>
    public partial class CreateDbView : UserControl
    {
        public CreateDbView()
        {
            InitializeComponent();
        }

        private void CreateVaultButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as CreateDbViewModel;
            if (vm == null) return;

            string vaultName = VaultNameInput.Text;
            string vaultPassword = VaultPasswordInput.Password;
            if(vaultName == "" || vaultPassword == "")
            {
                ShowPasswordStrenght.Text = "Please enter a password and a name for your vault";
                return;
            }

            ViewModel.CreateDbViewModel.CreateNewVault(vaultName, vaultPassword);
        }
       private void MyPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as System.Windows.Controls.PasswordBox;
            string currentPassword = passwordBox.Password;
            PasswordChecker PasswordChecker = new Model.PasswordChecker();
            string passwordStrength = PasswordChecker.checkPasswordStrength(currentPassword);
            if (passwordStrength == "password invalid")
                ShowPasswordStrenght.Text = "";
            else
                ShowPasswordStrenght.Text = passwordStrength;

        }

        
        }

        
    }

}
