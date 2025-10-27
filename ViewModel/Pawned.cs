using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace PasswordManager.ViewModel
{
    public class Pawned
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string PWNED_PASSWORDS_API = "https://api.pwnedpasswords.com/range/";

        
        public static async Task<int> CheckPasswordAsync(string password)
        {
            try
            {
                string hash = GetSHA1Hash(password);

                string prefix = hash.Substring(0, 5);
                string suffix = hash.Substring(5);

                string url = PWNED_PASSWORDS_API + prefix;
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "PasswordChecker");

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                
                var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2 && parts[0].Equals(suffix, StringComparison.OrdinalIgnoreCase))
                    {
                        return int.Parse(parts[1]);
                    }
                }

                return 0; 
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking password: {ex.Message}", ex);
            }
        }

        
        

        private static string GetSHA1Hash(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
        
}
}
