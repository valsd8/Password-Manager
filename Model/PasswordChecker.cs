using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Model
{
    public class PasswordChecker
    {
        public string checkPasswordStrength(string password)
        {
            int L = CalculateL(password);
            if (L == 0)
                return "password invalid";
            string resultat = ";";
            double entropy = Math.Log2(L) * password.Length;
            string msg = CheckStrenght((int)entropy);
            return msg;
        }
        private string CheckStrenght(int entropy)
        {

            if (entropy >= 128)
            {
                string msg = $"Really Strong Password, {entropy} bit of entropy";
                return msg;

            }
            if (entropy > 60)
            {
                string msg = $"Strong Password, {entropy} bit of entropy";
                return msg;
            }
            if(entropy > 40)
            {
                string msg = $"Average Password, {entropy} bit of entropy";
                return msg;
            }
            if (entropy > 15)
            {
                string msg = $"Weak Password, {entropy} bit of entropy";
                return msg;
            }
            else
            {
                string msg = $"Very Weak password, Please change it, only {entropy} bit of entropy";
                return msg;
            }
            

        }
        private int CalculateL(string password)
        {
            int L = 0;
            if (password.Any(c => char.IsLower(c))) L += 26;
            if (password.Any(c => char.IsUpper(c))) L += 26;
            if (password.Any(c => char.IsDigit(c))) L += 10;
            if (password.Any(c => "!@#$%^&*()_+-=[]{}|;:',.<>?/`~\"\\"
                                   .Contains(c))) L += 32;
            return L;
        }

    }
}
