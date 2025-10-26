using Password_Manager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Password_Manager;

namespace Password_Manager.ViewModel
{
    public class SharedDataServices
    {
        public string password {  get; set; }
        public EncryptedPackage EncryptedPackage { get; set; } = new EncryptedPackage();
        public Vault vault { get; set; } = new Vault();

        public Data data { get; set; }
        public byte[] aesKey { get; set; }

    }
}
