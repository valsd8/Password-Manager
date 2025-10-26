using Password_Manager.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Password_Manager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SharedDataServices SharedData { get; } = new SharedDataServices();
    }

}
