using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OPG_Robin_Strandberg_SYSM9.Models
{
    public class User : INotifyPropertyChanged
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }

        public virtual bool IsAdmin => false;

        public User(string userName, string password, string country)
        {
            UserName = userName;
            Password = password;
            Country = country;
        }

        public virtual bool ValidateLogin(string userName, string password)
        {
            return UserName == userName && Password == password;
        }

        public virtual void ChangePassword(string newPassword)
        {
            Password = newPassword;
            OnPropertyChanged(nameof(Password));
        }

        public virtual void UpdateDetails(string country)
        {
            Country = country;
            OnPropertyChanged(nameof(Country));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
