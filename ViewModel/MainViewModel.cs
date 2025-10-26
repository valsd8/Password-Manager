using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Password_Manager.ViewModel
{


    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        public EncryptedBinExistViewModel FileCheckerVM { get;  }

        private object _currentViewModel;
        public object CurrentViewModel
        {
            get => _currentViewModel;
            set 
            {
                _currentViewModel = value;
                OnPropertyChanged(); 
            }
        }

        public MainViewModel()
        {
            FileCheckerVM = new EncryptedBinExistViewModel();
            if (FileCheckerVM.CheckFile())
                CurrentViewModel = new EnterPasswordViewModel(NavigateTo);
            
            else
                CurrentViewModel = new CreateDbViewModel();

            

        }
        public void NavigateTo(object nextViewModel)
        {
            CurrentViewModel = nextViewModel;
        }
    }



}
