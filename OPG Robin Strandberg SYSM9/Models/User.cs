using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OPG_Robin_Strandberg_SYSM9.Models
{
    public class User : INotifyPropertyChanged
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }

        public bool IsAdmin { get; protected set; }

        public User(string userName, string password, string country)
        {
            UserName = userName;
            Password = password;
            Country = country;
        }

        public void ValidateLogin(string userName, string password)
        {
        }

        public void ChangePassword(string password)
        {
            Password = password;
        }

        public void UpdateDetails()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
