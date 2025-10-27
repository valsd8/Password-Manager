using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Model
{
    public class Data
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public string OptionalComment { get; set; }
        //public int Id { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString();

    }
}
