using PasswordManager.Model;
using PasswordManager.ViewModel;
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

namespace PasswordManager.View
{
    /// <summary>
    /// Logique d'interaction pour CreateDbView.xaml
    /// </summary>
    public partial class CreateDbView : UserControl
    {
        public CreateDbView()
        {
            InitializeComponent();
            this.Loaded += CreateDbView_Loaded;
        }
        private void CreateDbView_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                
                parentWindow.Icon = CreateColoredCircleIcon("#eb6734");
            }
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
        public DrawingImage CreateColoredCircleIcon(string hexColor)
        {
            var color = (Color)ColorConverter.ConvertFromString(hexColor);

            return new DrawingImage(
                new GeometryDrawing(
                    new SolidColorBrush(color),
                    null,
                    new EllipseGeometry(new Point(8, 8), 8, 8)
                )
            );
        }
        private void MyPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as System.Windows.Controls.PasswordBox;
            string currentPassword = passwordBox.Password;
            PasswordChecker PasswordChecker = new Model.PasswordChecker();
            string passwordStrength = PasswordChecker.checkPasswordStrength(currentPassword);
            

            DisplayBreachedPassword(currentPassword);
            if (currentPassword == "")
                ShowPawnedPassword.Text = "";

            if (passwordStrength == "password invalid")
                ShowPasswordStrenght.Text = "";
            else if (passwordStrength.StartsWith("Really Strong Password"))
            {
                VaultPasswordInput.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#04d484"));
                ShowPasswordStrenght.Text = passwordStrength;
            }
            else if (passwordStrength.StartsWith("Strong Password"))
            {
                ShowPasswordStrenght.Text = passwordStrength;
                VaultPasswordInput.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#87ed95"));
            }
            else if (passwordStrength.StartsWith("Average Password"))
            {
                ShowPasswordStrenght.Text = passwordStrength;
                VaultPasswordInput.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#deeb54"));
            }
            else if (passwordStrength.StartsWith("Very Weak password"))
            {
                ShowPasswordStrenght.Text = passwordStrength;
                VaultPasswordInput.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e61e1e"));

            }
            else if (passwordStrength.StartsWith("Weak Password"))
            {
                ShowPasswordStrenght.Text = passwordStrength;
                VaultPasswordInput.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#eb8c54"));

            }
            else
                ShowPasswordStrenght.Text = passwordStrength;

        }
        public async void DisplayBreachedPassword(string password)
        {
            Console.WriteLine($"Checking: {new string('*', password.Length)}");

            if (string.IsNullOrEmpty(password))
            {
                ShowPawnedPassword.Text = "";
                return;
            }

            int breachCount = await Pawned.CheckPasswordAsync(password);

            if (breachCount > 0)
            {
                ShowPawnedPassword.Text = $"Please dont use this password. It has been found in {breachCount} data breaches.";
            }
            else
            {
                ShowPawnedPassword.Text = ""; 
            }
        }



    }

        
    }


