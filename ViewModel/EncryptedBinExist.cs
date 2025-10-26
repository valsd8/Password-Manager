using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Password_Manager.ViewModel
{
    public class EncryptedBinExistViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private bool _isBinaryFileExists;
        public bool IsBinaryFileExists
        {
            get => _isBinaryFileExists;
            set
            {
                _isBinaryFileExists = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FileExistsMessage));
            }
        }

        public string FileExistsMessage => IsBinaryFileExists ? "File found!" : "File NOT found!";

        public EncryptedBinExistViewModel()
        {
            CheckFile();
        }

        public bool CheckFile()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "encrypted.bin");
            IsBinaryFileExists = File.Exists(filePath);
            return IsBinaryFileExists;
        }
    }

}
