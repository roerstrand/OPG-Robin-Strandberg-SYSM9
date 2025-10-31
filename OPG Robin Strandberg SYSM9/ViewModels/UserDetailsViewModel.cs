using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Commands;

namespace OPG_Robin_Strandberg_SYSM9.ViewModels
{
    public class UserDetailsViewModel : INotifyPropertyChanged
    {
        private readonly UserManager _userManager;

        public string CurrentUserName => _userManager.CurrentUser.UserName;
        public string CurrentCountry => _userManager.CurrentUser.Country;

        private string _newUserName;
        public string NewUserName
        {
            get => _newUserName;
            set { _newUserName = value; OnPropertyChanged(); }
        }

        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public ObservableCollection<string> Countries { get; }
        private string _selectedCountry;
        public string SelectedCountry
        {
            get => _selectedCountry;
            set { _selectedCountry = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public UserDetailsViewModel(UserManager userManager)
        {
            _userManager = userManager;

            Countries = new ObservableCollection<string>
            {
                "Sweden", "Norway", "Finland", "Denmark", "Germany"
            };

            SaveCommand = new RelayCommand(_ => SaveChanges());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private void SaveChanges()
        {
            if (!string.IsNullOrWhiteSpace(NewUserName))
            {
                if (NewUserName.Length < 3)
                {
                    MessageBox.Show("Username must be at least 3 characters long.");
                    return;
                }

                if (_userManager.IsUsernameTaken(NewUserName))
                {
                    MessageBox.Show("That username is already taken.");
                    return;
                }

                _userManager.CurrentUser.UserName = NewUserName;
            }

            if (!string.IsNullOrWhiteSpace(NewPassword))
            {
                if (NewPassword.Length < 5)
                {
                    MessageBox.Show("Password must be at least 5 characters long.");
                    return;
                }

                if (NewPassword != ConfirmPassword)
                {
                    MessageBox.Show("Passwords do not match.");
                    return;
                }

                _userManager.CurrentUser.Password = NewPassword;
            }

            if (!string.IsNullOrWhiteSpace(SelectedCountry))
                _userManager.CurrentUser.Country = SelectedCountry;

            MessageBox.Show("User details updated successfully!");

            Application.Current.MainWindow.Content = new Views.RecipeListUserControl(_userManager.GetRecipeManagerForCurrentUser());
        }

        private void Cancel()
        {
            Application.Current.MainWindow.Content = new Views.RecipeListUserControl(_userManager.GetRecipeManagerForCurrentUser());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
