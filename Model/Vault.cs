using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Model
{
    public class Vault
    {
        public string Name { get; set; }   
        public List<Data> Entries { get; set; } = new List<Data>();
    }

}
