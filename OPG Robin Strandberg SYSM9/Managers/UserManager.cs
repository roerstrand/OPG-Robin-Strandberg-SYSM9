using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OPG_Robin_Strandberg_SYSM9.Managers
{
    internal class UserManager
    {

public class UserManager : Managers
    {
        public class UserManager : INotifyPropertyChanged
        {
            private List<User> _users;

            public List<User> Users;
        {
                get
                {
                    return _users;
                }
                set
                {
                    _users = value;
                }
            }

            public UserManager()
            {
                CreateDefaultUsers();
                _users = new List<User>();
            }

            public void CreateDefaultUsers()
            {
                Users.Add(new User
                {
                    UserName = "admin",
                    DisplayName = "Administratör",
                    Role = "Admin",
                    Password = "1234",
                });

                Users.Add(new User
                {
                    UserName = "User",
                    DisplayName = "Regular user",
                    Role = "User",
                    Password = "1234",
                });
            }


            private User? _currentUser;

            public User? CurrentUser
            {
                get { return _currentUser; }
                set
                {
                    _currentUser = value;
                    OnPropertyChanged();
                }
            }

            public bool Login(string username, string password)
            {
                foreach (User u in Users)
                {
                    if (u.UserName == username && u.Password = password)
                    {
                        CurrentUser = u;
                        return true;
                    }
                }

                return false;
            }

            public void Logout()
            {
                CurrentUser = null;
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventsArgs(propertyName))
            }
        }
    }
}
}
