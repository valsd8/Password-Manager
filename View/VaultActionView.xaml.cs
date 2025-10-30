using Password_Manager;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordManager.View
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
            this.SizeChanged += VaultListView_SizeChanged;
            VaultListView.ContextMenuOpening += VaultListView_ContextMenuOpening;
            DisplayData data = new DisplayData();
            data.displayData(VaultListView);
        }
        private void VaultListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            
            var element = e.OriginalSource as DependencyObject;
            var listViewItem = FindParent<ListViewItem>(element);

            if (listViewItem == null)
            {
                e.Handled = true; 
            }
        }

        
        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        private void VaultAction_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                var VaultAction = new ViewModel.VaultAction();
                parentWindow.Title = VaultAction.SetWindowsTitle();
                
                parentWindow.Icon = CreateColoredCircleIcon("#2196F3");
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
        private void CopyUrl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = sender as MenuItem;
                if (menuItem != null)
                {
                    var contextMenu = menuItem.Parent as ContextMenu;
                    var listViewItem = contextMenu?.PlacementTarget as ListViewItem;
                    var entry = listViewItem?.Content as Data;

                    if (entry != null && !string.IsNullOrEmpty(entry.Url))
                    {
                        Clipboard.SetText(entry.Url);
                        ShowCopyNotification("URL copied!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CopyEmail_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var contextMenu = menuItem.Parent as ContextMenu;
                var listViewItem = contextMenu?.PlacementTarget as ListViewItem;
                var entry = listViewItem?.Content as Data;

                if (entry != null && !string.IsNullOrEmpty(entry.Email))
                {
                    Clipboard.SetText(entry.Email);
                    ShowCopyNotification("Email copied!");
                }
            }
        }

        private void CopyPassword_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var contextMenu = menuItem.Parent as ContextMenu;
                var listViewItem = contextMenu?.PlacementTarget as ListViewItem;
                var entry = listViewItem?.Content as Data;

                if (entry != null && !string.IsNullOrEmpty(entry.Password))
                {
                    Clipboard.SetText(entry.Password);
                    ShowCopyNotification("Password copied!");
                }
            }
        }

        private void CopyComment_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var contextMenu = menuItem.Parent as ContextMenu;
                var listViewItem = contextMenu?.PlacementTarget as ListViewItem;
                var entry = listViewItem?.Content as Data;

                if (entry != null && !string.IsNullOrEmpty(entry.OptionalComment))
                {
                    Clipboard.SetText(entry.OptionalComment);
                    ShowCopyNotification("Comment copied!");
                }
            }
        }


        private void ShowCopyNotification(string message)
        {
            // Option 1: Simple MessageBox
            MessageBox.Show(message, "Copied", MessageBoxButton.OK, MessageBoxImage.Information);

            // Option 2: Or use a status bar / notification area if you have one
            // StatusText.Text = message;
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
            DisplayData DisplayData = new DisplayData();
            DisplayData.DisplayBreachedPassword(password, urlEntry);
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
            VaultListView.Width = this.ActualWidth - 40;
        }

        private void ListViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            var listViewItem = sender as ListViewItem;
            if (listViewItem != null)
            {
                // Créer un nouveau ContextMenu pour chaque item
                var contextMenu = new ContextMenu
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3")),
                    BorderThickness = new Thickness(1),
                    Padding = new Thickness(4),
                    HasDropShadow = true
                };

                // Style pour les MenuItems
                var menuItemStyle = new Style(typeof(MenuItem));
                menuItemStyle.Setters.Add(new Setter(MenuItem.HeightProperty, 32.0));
                menuItemStyle.Setters.Add(new Setter(MenuItem.PaddingProperty, new Thickness(10, 5, 10, 5)));
                menuItemStyle.Setters.Add(new Setter(MenuItem.FontSizeProperty, 13.0));
                menuItemStyle.Setters.Add(new Setter(MenuItem.CursorProperty, Cursors.Hand));

                var trigger = new Trigger { Property = MenuItem.IsHighlightedProperty, Value = true };
                trigger.Setters.Add(new Setter(MenuItem.BackgroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E3F2FD"))));
                trigger.Setters.Add(new Setter(MenuItem.ForegroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1976D2"))));
                menuItemStyle.Triggers.Add(trigger);

                contextMenu.Resources.Add(typeof(MenuItem), menuItemStyle);

                // Créer les MenuItems
                var copyUrlItem = new MenuItem { Header = "📋 Copy URL" };
                copyUrlItem.Click += CopyUrl_Click;

                var copyEmailItem = new MenuItem { Header = "📋 Copy Email" };
                copyEmailItem.Click += CopyEmail_Click;

                var copyPasswordItem = new MenuItem { Header = "📋 Copy Password" };
                copyPasswordItem.Click += CopyPassword_Click;

                var copyCommentItem = new MenuItem { Header = "📋 Copy Comment" };
                copyCommentItem.Click += CopyComment_Click;

                contextMenu.Items.Add(copyUrlItem);
                contextMenu.Items.Add(copyEmailItem);
                contextMenu.Items.Add(copyPasswordItem);
                contextMenu.Items.Add(copyCommentItem);

                listViewItem.ContextMenu = contextMenu;
            }
        }

    }
}
