using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using OPG_Robin_Strandberg_SYSM9.Models;

namespace OPG_Robin_Strandberg_SYSM9.Managers
{
    public class UserManager : INotifyPropertyChanged
    {
        private User _currentUser;

        public User CurrentUser
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

        private readonly Dictionary<User, RecipeManager> _userRecipeManagers = new();

        private List<User> _users;

        public List<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }

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
            Users.Add(new User("admin", "password", "Sweden"));
            Users.Add(new User("user", "password", "Norway"));
        }

        public bool Login(string username, string password)
        {
            try
            {
                foreach (User u in Users)
                {
                    if (u.UserName == username && u.Password == password)
                    {
                        CurrentUser = u;
                        IsAuthenticated = true;
                        GetRecipeManagerForCurrentUser();
                        MessageBox.Show($"Welcome {u.UserName}!");
                        OnPropertyChanged(nameof(IsAuthenticated));
                        return true;
                    }
                }

                IsAuthenticated = false;
                OnPropertyChanged(nameof(IsAuthenticated));
                MessageBox.Show("Warning! Wrong password or username.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Unexpected error when trying to login.");
            }

            return false;
        }

        public RecipeManager GetRecipeManagerForCurrentUser()
        {
            try
            {
                if (CurrentUser == null)
                    return null;

                if (!_userRecipeManagers.ContainsKey(CurrentUser))
                    _userRecipeManagers[CurrentUser] = new RecipeManager();

                return _userRecipeManagers[CurrentUser];
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }


        public void Logout()
        {
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

                // Kontrollera om lösenordet matchar mönstret
                if (!Regex.IsMatch(newPassword, pattern))
                {
                    MessageBox.Show(
                        "Lösenordet måste vara minst 8 tecken långt och innehålla minst en siffra och ett specialtecken.",
                        "Ogiltigt lösenord",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                // Uppdatera användarens lösenord om det är giltigt
                user.Password = newPassword;

                MessageBox.Show(
                    "Lösenordet har ändrats!",
                    "Klart",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ett fel uppstod vid ändring av lösenordet:\n" + ex.Message,
                    "Systemfel",
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
                if (Users.Any(u =>
                        u.UserName.Equals(newUserName, StringComparison.Ordinal)))
                {
                    MessageBox.Show("Username already taken!.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Unexpected error when checking if username already taken. Please try another username.");
            }
            return true;
        }

        public List<User> FindUsers(string username)
        {
            try
            {
                List<User> foundUsers = new List<User>();

                foreach (User u in Users)
                {
                    if (username == u.UserName)
                    {
                        foundUsers.Add(u);
                    }
                }

                return foundUsers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
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


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
