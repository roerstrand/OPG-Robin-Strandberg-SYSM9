using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OPG_Robin_Strandberg_SYSM9.Models;

namespace OPG_Robin_Strandberg_SYSM9.Managers
{
    internal class UserManager : INotifyPropertyChanged
    {
        private User _loggedIn;

        public User LoggedIn;
        {
            get => _loggedIn;
        }

        private set
        {
            _currentUser = value;
            OnPropertyChanged(LoggedIn);
            OnPropertyChanged(IsAuthenticated);
        }

        private readonly List<User> _users;

        public List<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(Users);
            }
        }

        public bool IsAuthenticated => CurrentUser != null;

        public UserManager()
        {
            _users = new List<User>();
        }

        public void CreateDefaultUsers()
        {
            Users.Add(new User
            {
                UserName = "admin",
                Password = "password",
                Country = "",
            });

            Users.Add(new User
            {
                UserName = "user",
                Password = "password",
                Country = "",
            });
        }

        public Register(string username, string password, string country)
        {
            Users.Add(new User
            {
                UserName = username,
                Password = password,
                Country = country,
            });
        }

        public findUser(string username)
        {
            foreach (user u in _users)
            {
                if (username == UserName)
                {
                    Console.WriteLine($"Found user: {username}.");
                    return username;
                }
            }
        }

        public changePassword(string username, string password)
        {
            for (user u in
            _users)
            {
                u.UserName = username;
                u.Password = password;
            }
        }

        public getLoggedIn()
        {
            return LoggedIn;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string n)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
        }
    }
}