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
    /// Logique d'interaction pour EnterPasswordView.xaml
    /// </summary>
    public partial class EnterPasswordView : UserControl
    {
        public EnterPasswordView()
        {
            InitializeComponent();
            this.Loaded += EnterPasswordView_Loaded;


        }
        private void EnterPasswordView_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                var VaultAction = new ViewModel.VaultAction();
                parentWindow.Title = VaultAction.SetWindowsTitle();
            }
        }

        
    }
}
