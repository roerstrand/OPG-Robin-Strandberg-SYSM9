using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPG_Robin_Strandberg_SYSM9.Models
{
    internal class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }

        public User(string userName, string password, string country)
        {
            UserName = userName;
            Password = password;
            Country = country;
        }

        public void ValidateLogin(string userName, string password)
        {
           
        }


    }
}
