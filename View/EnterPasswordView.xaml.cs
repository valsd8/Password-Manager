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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordManager.View
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
                string title = $"{VaultAction.SetWindowsTitle()} (locked)";
                parentWindow.Title = title;
                parentWindow.Icon = CreateColoredCircleIcon("#ebd834");
            }
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


    }
}
