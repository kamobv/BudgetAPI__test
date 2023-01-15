using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAPI.Helpers
{
    public static class Helper
    {
        public static string HashPassword(string password)
        {
            byte[] bytePass = Encoding.ASCII.GetBytes(password);
            byte[] hashPass = new SHA256Managed().ComputeHash(bytePass);

            return Encoding.ASCII.GetString(hashPass);
        }
        public static bool CehckPassword(string userPassword,string hashPassword)
        {
            return HashPassword(userPassword) == hashPassword;
        }
    }
}
