using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace OPG_Robin_Strandberg_SYSM9.Models
{
    public class User : INotifyPropertyChanged
    {
        private string _userName;
        private string _password;
        private string _country;

        public string UserName
        {
            get => _userName;
            private set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            private set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string Country
        {
            get => _country;
            private set
            {
                _country = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Recipe> RecipeList { get; set; }

        public virtual bool IsAdmin => false;

        public User(string userName, string password, string country)
        {
            _userName = userName;
            _password = password;
            _country = country;
            RecipeList = new ObservableCollection<Recipe>();
        }

        public virtual bool ValidateLogin(string userName, string password)
        {
            return _userName == userName && _password == password;
        }

        public virtual void ChangeUserName(string newUserName)
        {
            if (string.IsNullOrWhiteSpace(newUserName))
            {
                MessageBox.Show("Username cannot be empty.",
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newUserName.Length < 3)
            {
                MessageBox.Show("Username must be at least 3 characters long.",
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            UserName = newUserName.Trim();
            MessageBox.Show("Username updated successfully.",
                "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public virtual void ChangePassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Password cannot be empty.",
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword.Length < 5)
            {
                MessageBox.Show("Password must be at least 5 characters long.",
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Password = newPassword.Trim();
            MessageBox.Show("Password updated successfully.",
                "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public virtual void UpdateDetails(string newCountry)
        {
            if (string.IsNullOrWhiteSpace(newCountry))
            {
                MessageBox.Show("Country cannot be empty.",
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Country = newCountry.Trim();
            MessageBox.Show("Country updated successfully.",
                "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
