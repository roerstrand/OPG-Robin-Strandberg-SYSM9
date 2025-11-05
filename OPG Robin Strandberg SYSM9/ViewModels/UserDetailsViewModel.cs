using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using OPG_Robin_Strandberg_SYSM9.Commands;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Views;

namespace OPG_Robin_Strandberg_SYSM9.ViewModels
{
    public class UserDetailsViewModel : INotifyPropertyChanged
    {
        private readonly UserManager _userManager;

        public string CurrentUserName => _userManager.CurrentUser.UserName;
        public string CurrentCountry => _userManager.CurrentUser.Country;

        private string _newUserName;
        private string _selectedCountry;

        public string NewUserName
        {
            get => _newUserName;
            set
            {
                _newUserName = value;
                OnPropertyChanged();
            }
        }

        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public ObservableCollection<string> Countries { get; }

        public string SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                _selectedCountry = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public UserDetailsViewModel(UserManager userManager)
        {
            if (userManager == null || userManager.CurrentUser == null)
            {
                MessageBox.Show("No user is currently logged in.", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            _userManager = userManager;

            Countries = new ObservableCollection<string>
            {
                "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Antigua and Barbuda",
                "Argentina", "Armenia", "Australia", "Austria", "Azerbaijan", "Bahamas",
                "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize",
                "Benin", "Bhutan", "Bolivia", "Bosnia and Herzegovina", "Botswana",
                "Brazil", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia",
                "Cameroon", "Canada", "Chile", "China", "Colombia", "Costa Rica", "Croatia",
                "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominican Republic",
                "Ecuador", "Egypt", "El Salvador", "Estonia", "Ethiopia", "Finland",
                "France", "Germany", "Ghana", "Greece", "Greenland", "Guatemala", "Honduras",
                "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland",
                "Israel", "Italy", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya",
                "Kuwait", "Latvia", "Lebanon", "Liberia", "Libya", "Lithuania", "Luxembourg",
                "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Mexico",
                "Moldova", "Monaco", "Mongolia", "Montenegro", "Morocco", "Mozambique",
                "Namibia", "Nepal", "Netherlands", "New Zealand", "Nicaragua", "Niger",
                "Nigeria", "North Korea", "Norway", "Oman", "Pakistan", "Palestine",
                "Panama", "Paraguay", "Peru", "Philippines", "Poland", "Portugal",
                "Qatar", "Romania", "Russia", "Rwanda", "Saudi Arabia", "Senegal",
                "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia",
                "South Africa", "South Korea", "Spain", "Sri Lanka", "Sudan", "Sweden",
                "Switzerland", "Syria", "Taiwan", "Tanzania", "Thailand", "Tunisia",
                "Turkey", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom",
                "United States", "Uruguay", "Uzbekistan", "Venezuela", "Vietnam", "Yemen",
                "Zambia", "Zimbabwe"
            };

            SaveCommand = new RelayCommand(_ => SaveChanges());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private void SaveChanges()
        {
            try
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
                {
                    _userManager.CurrentUser.Country = SelectedCountry;
                }

                MessageBox.Show("User details updated successfully!", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);

                var listWindow = new RecipeListWindow(_userManager.GetRecipeManagerForCurrentUser());
                listWindow.Show();

                foreach (Window w in Application.Current.Windows)
                {
                    if (w.DataContext == this)
                    {
                        w.Close();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            try
            {
                var listWindow = new RecipeListWindow(_userManager.GetRecipeManagerForCurrentUser());
                listWindow.Show();

                foreach (Window w in Application.Current.Windows)
                {
                    if (w.DataContext == this)
                    {
                        w.Close();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
