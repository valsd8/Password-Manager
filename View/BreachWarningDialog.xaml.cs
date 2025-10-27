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

using System.Windows;

namespace PasswordManager.View
{
    public partial class BreachWarningDialog : Window
    {
        public BreachWarningDialog(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
            this.Loaded += BreachWarning_Loaded;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        public static void Show(string message)
        {
            var dialog = new BreachWarningDialog(message);
            dialog.ShowDialog();
        }

        
        public static void Show(string message, string title)
        {
            var dialog = new BreachWarningDialog(message);
            dialog.Title = title;
            dialog.ShowDialog();
        }
        private void BreachWarning_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                var VaultAction = new ViewModel.VaultAction();
                parentWindow.Title = VaultAction.SetWindowsTitle();

                parentWindow.Icon = CreateColoredCircleIcon("#de0d0d");
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
