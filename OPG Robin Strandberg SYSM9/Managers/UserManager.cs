using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OPG_Robin_Strandberg_SYSM9.Models;

namespace OPG_Robin_Strandberg_SYSM9.Managers
{
    public class UserManager : INotifyPropertyChanged
    {
        private User _loggedIn;

        public User LoggedIn
        {
            get { return _loggedIn; }


            private set
            {
                _loggedIn = value;
                OnPropertyChanged();
            }
        }

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
            Users.Add(new User("admin", "password", ""));
            Users.Add(new User("user", "password", ""));
        }

        public bool Login(string username, string password)
        {
            foreach (User u in Users)
            {
                if (u.UserName == username && u.Password == password)
                {
                    LoggedIn = u;
                    IsAuthenticated = true;
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

        public void Logout()
        {
            LoggedIn = null;
            IsAuthenticated = false;
            OnPropertyChanged(nameof(IsAuthenticated));
        }


        public void Register(string username, string password, string country)
        {
            Users.Add(new User(username, password, country));
        }

        public List<User> FindUser(string username)
        {
            List<User> foundUsers = new List<User>();

            foreach (User u in Users)
            {
                if (username == u.UserName)
                {
                    Console.WriteLine($"Found user: {username}.");
                    foundUsers.Add(u);
                }
            }

            return foundUsers;
        }

        public void ChangePassword(string username, string password)
        {
            foreach (User u in Users)
            {
                u.UserName = username;
                u.Password = password;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
