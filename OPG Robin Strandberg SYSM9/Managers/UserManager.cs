using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Text.Json;
using OPG_Robin_Strandberg_SYSM9.Models;

namespace OPG_Robin_Strandberg_SYSM9.Managers
{
    public class UserManager : INotifyPropertyChanged
    {
        private User _currentUser;

        public User CurrentUser // getLogged in här
        {
            get { return _currentUser; }

            private set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        private User _searchedUser;

        public User SearchedUser
        {
            get { return _searchedUser; }


            set
            {
                _searchedUser = value;
                OnPropertyChanged();
            }
        }

        // Dictionary med alla användares recept
        private readonly Dictionary<User, RecipeManager> _userRecipeManagers = new();

        private List<User> _users;

        public List<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        // Lista med aktiva admins
        public List<AdminUser> ActiveAdmins { get; private set; } = new();

        private bool _isAuthenticated;

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                if (_isAuthenticated != value)
                {
                    _isAuthenticated = value;
                    OnPropertyChanged();
                }
            }
        }

        public UserManager()
        {
            Users = new List<User>();
            CreateDefaultUsers();
        }

        public void CreateDefaultUsers()
        {
            Users.Add(new AdminUser("admin", "password", "Sweden"));
            var normalUser = new User("user", "password", "Norway");
            Users.Add(normalUser);

            normalUser.RecipeList.Add(new Recipe(
                "Classic Pancakes",
                "Mix flour, milk, eggs, and butter. Fry in pan until golden.",
                "Breakfast",
                DateTime.Now,
                normalUser,
                "Flour, Milk, Eggs, Butter, Salt"
            ));

            normalUser.RecipeList.Add(new Recipe(
                "Spaghetti Bolognese",
                "Cook pasta. Prepare sauce with minced meat, tomatoes, and herbs. Combine and serve.",
                "Dinner",
                DateTime.Now,
                normalUser,
                "Spaghetti, Minced Meat, Tomato Sauce, Garlic, Onion, Herbs"
            ));
        }

        public bool Login(string username, string password)
        {
            try
            {
                foreach (User u in Users)
                {
                    if (u.UserName == username && u.Password == password)
                    {
                        if (!PerformTwoFactorAuthentication(u))
                            return false;

                        CurrentUser = u;
                        IsAuthenticated = true;
                        GetRecipeManagerForCurrentUser();

                        if (u is AdminUser admin && !ActiveAdmins.Contains(admin))
                        {
                            ActiveAdmins.Add(admin);
                            MessageBox.Show($"Welcome administrator {u.UserName}!",
                                "Login successful", MessageBoxButton.OK, MessageBoxImage.Information);
                            OnPropertyChanged(nameof(IsAuthenticated));
                            return true;
                        }

                        MessageBox.Show($"Welcome {u.UserName}!",
                            "Login successful", MessageBoxButton.OK, MessageBoxImage.Information);
                        OnPropertyChanged(nameof(IsAuthenticated));
                        return true;
                    }
                }

                IsAuthenticated = false;
                OnPropertyChanged(nameof(IsAuthenticated));
                MessageBox.Show("Incorrect username or password.",
                    "Login failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Unexpected error occurred while trying to log in.",
                    "System error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public RecipeManager GetRecipeManagerForCurrentUser()
        {
            try
            {
                if (CurrentUser == null)
                    return null;

                if (!_userRecipeManagers.ContainsKey(CurrentUser))
                    _userRecipeManagers[CurrentUser] = new RecipeManager(CurrentUser);

                return _userRecipeManagers[CurrentUser];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }


        public void Logout()
        {
            if (CurrentUser is AdminUser admin && ActiveAdmins.Contains(admin))
            {
                ActiveAdmins.Remove(admin);
            }

            CurrentUser = null;
            IsAuthenticated = false;
            OnPropertyChanged(nameof(IsAuthenticated));
        }

        public void ChangePassword(User user, string newPassword)
        {
            try
            {
                // Regex-mönster
                //
                // ^                           → Början av strängen
                // =?.                         → Lookahead för specifikt mönster
                // (?=.*\d)                    → Måste innehålla minst en siffra (0–9)
                // (?=.*[!@#$%^&*(),.?""':{}|<>]) → Måste innehålla minst ett specialtecken
                // [A-Za-z\d!@#$%^&*(),.?""':{}|<>]{8,} → Tillåtna tecken (bokstäver, siffror och specialtecken) samt minst 8 tecken totalt
                // $                           → Slutet av strängen
                string pattern = @"^(?=.*\d)(?=.*[!@#$%^&*(),.?""':{}|<>])[A-Za-z\d!@#$%^&*(),.?""':{}|<>]{8,}$";

                if (!Regex.IsMatch(newPassword, pattern))
                {
                    MessageBox.Show(
                        "Password must be at least 8 characters long and include at least one digit and one special character.",
                        "Invalid password",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                user.ChangePassword(newPassword);

                MessageBox.Show(
                    "Password has been changed!",
                    "Done",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error occurred during registration:\n" + ex.Message,
                    "System error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        public bool Register(string username, string password, string country)
        {
            try
            {
                // samma string pattern som vid ChangePassword
                string pattern = @"^(?=.*\d)(?=.*[!@#$%^&*(),.?""':{}|<>])[A-Za-z\d!@#$%^&*(),.?""':{}|<>]{8,}$";

                if (!Regex.IsMatch(password, pattern))
                {
                    MessageBox.Show(
                        "Password must be 8 symbols long, contain at least one digit and one special character.",
                        "Not allowed password",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return false;
                }

                if (Users.Any(u => u.UserName.Equals(username, StringComparison.Ordinal)))
                {
                    MessageBox.Show(
                        "Username already taken. Please choose another.",
                        "Username taken",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return false;
                }

                Users.Add(new User(username, password, country));

                MessageBox.Show(
                    "Registration succeeded!!",
                    "Done",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error occured during registration:\n" + ex.Message,
                    "System error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return false;
            }
        }

        public bool IsUsernameTaken(string newUserName)
        {
            try
            {
                return Users.Any(u =>
                    u.UserName.Equals(newUserName, StringComparison.Ordinal));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Unexpected error when checking username availability.");
                return false;
            }
        }

        public User FindUser(string username)
        {
            try
            {
                foreach (User u in Users)
                {
                    if (u.UserName == username)
                    {
                        return SearchedUser = u;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private string _lastGeneratedCode;

        // Simulerad 2FA med messagebox för 6-siffrig kod
        private bool PerformTwoFactorAuthentication(User user)
        {
            try
            {
                var random = new Random();
                _lastGeneratedCode = random.Next(100000, 999999).ToString();

                MessageBox.Show(
                    $"Simulated email sent to {user.UserName}@example.com\nVerification code: {_lastGeneratedCode}",
                    "Two-Factor Authentication",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                var twoFactorWindow = new OPG_Robin_Strandberg_SYSM9.Views.TwoFactorWindow(_lastGeneratedCode);
                bool? result = twoFactorWindow.ShowDialog();

                if (result == true)
                {
                    return true;
                }

                MessageBox.Show("Two-Factor verification failed. Login cancelled.",
                    "Verification failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during two-factor authentication:\n" + ex.Message,
                    "System error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
