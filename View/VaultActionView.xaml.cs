using Password_Manager;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Password_Manager.View
{
    /// <summary>
    /// Logique d'interaction pour VaultAction.xaml
    /// </summary>
    public partial class VaultAction : UserControl
    {
        public static ListView StaticVaultListView { get; private set; }
        public VaultAction()
        {
            InitializeComponent();
            StaticVaultListView = VaultListView;
            this.Loaded += VaultAction_Loaded;
            DisplayData data = new DisplayData();
            data.displayData(VaultListView);
        }

        private void VaultAction_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                var VaultAction = new ViewModel.VaultAction();
                parentWindow.Title = VaultAction.SetWindowsTitle();
            }
        }

        

        public void Add_entry_btn_Click(object sender, RoutedEventArgs e)
        {
            
            var vaultAction = new ViewModel.VaultAction();
            string urlEntry = url.Text;
            string usernameEntry = username.Text;
            string password = NewPasswordBox.Password;
            string comment = OptionalComment.Text;

            if (string.IsNullOrWhiteSpace(usernameEntry) || string.IsNullOrWhiteSpace(password))
            {
                errorTextBlock.Text = "Please enter at least a username and a password.";
                errorTextBlock.Foreground = Brushes.Red;
                return;
            }
            string inputValid = vaultAction.ValidateInput(urlEntry, usernameEntry, password, comment);
            if (inputValid != null)
            {
                errorTextBlock.Text = inputValid;
                errorTextBlock.Foreground = Brushes.Red;
                return;
            }
            url.Text = "";
            username.Text = "";
            NewPasswordBox.Password = "";
            OptionalComment.Text = "";
            vaultAction.addEntry(usernameEntry, password, comment, urlEntry);
        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Data entry = btn.DataContext as Data;
            if (entry != null)
            {
                var vaultAction = new ViewModel.VaultAction();
                vaultAction.DeleteEntry(entry.Id);
                //MessageBox.Show($"Deleting entry for {entry.Id}");
                
            }
        }

        private void VaultListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //double totalWidth = VaultListView.ActualWidth - 35; // subtract a little for scrollbar & margin

            //if (VaultGridView.Columns.Count >= 4)
            {
                //VaultGridView.Columns[0].Width = totalWidth * 0.25; // 25%
                //VaultGridView.Columns[1].Width = totalWidth * 0.25; // 25%
                //VaultGridView.Columns[2].Width = totalWidth * 0.20; // 20%
                //VaultGridView.Columns[3].Width = totalWidth * 0.30; // 30%
            }
        }

    }
}
