using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Commands;
using OPG_Robin_Strandberg_SYSM9.Models;

namespace OPG_Robin_Strandberg_SYSM9
{
    public class ForgotPasswordViewModel : INotifyPropertyChanged
    {
        private readonly UserManager _userManager;

        // Har här övergått till samlad lista av alla fält först från tidigare fält+egenskap för sig
        private string _inputUsername;
        private string _inputNewPassword;
        private string _inputConfirmPassword;
        private string _lastFourPreviousPassword;

        private int _attempts;
        private bool _canSubmit;

        public string InputUsername
        {
            get => _inputUsername;
            set
            {
                _inputUsername = value;
                OnPropertyChanged();
            }
        }

        public string InputNewPassword
        {
            get => _inputNewPassword;
            set
            {
                _inputNewPassword = value;
                OnPropertyChanged();
            }
        }

        public string InputConfirmNewPassword
        {
            get => _inputConfirmPassword;
            set
            {
                _inputConfirmPassword = value;
                OnPropertyChanged();
            }
        }

        public string LastFourPreviousPassword
        {
            get => _lastFourPreviousPassword;
            set
            {
                _lastFourPreviousPassword = value;
                OnPropertyChanged();
            }
        }

        public int Attempts
        {
            get => _attempts;
            set
            {
                _attempts = value;
                OnPropertyChanged();
            }
        }

        public bool CanSubmit
        {
            get => _canSubmit;
            set
            {
                if (value != _canSubmit)
                {
                    _canSubmit = value;
                    OnPropertyChanged();
                }
            }
        }

        // kommando kopplat till knappen
        public RelayCommand SubmitCommand { get; }

        public ForgotPasswordViewModel()
        {
            _userManager = App.UserManager;
            SubmitCommand = new RelayCommand(o => ValidatePriorPasswordChange());
        }

        private void ChangePassword(User user, string newPassword)
        {
            try
            {
                User Currentuser = null;

                foreach (User u in _userManager.Users)
                {
                    if (u.UserName == user.UserName)
                    {
                        Currentuser = u;
                        break;
                    }
                }

                if (Currentuser == null)
                {
                    MessageBox.Show("User not found");
                    return;
                }

                if (InputNewPassword != newPassword)
                {
                    MessageBox.Show("Passwords do not match");
                    return;
                }

                Currentuser.Password = newPassword;
            }
            catch (Exception)
            {
                Console.WriteLine("Unexpected error occurred in method CHangePassword in ForgotPasswordViewModel");
            }
        }

        private void ValidatePriorPasswordChange()
        {
            try
            {
                var user = _userManager.FindUser(InputUsername);

                if (user == null)
                {
                    MessageBox.Show("User not found");
                    return;
                }

                var password = user.Password;
                var lastFour = password.Substring(password.Length - 4);
                lastFour = lastFour.Trim(); // Extra säkerhet ifall lagrat lösenord innehåller mellanslag

                int remaining = 3 - Attempts;

                if (lastFour != LastFourPreviousPassword)
                {
                    Attempts++;
                    if (Attempts == 3)
                    {
                        CanSubmit = false;
                        return;
                    }

                    MessageBox.Show(
                        $"Security check does not match current password. {remaining} attempts to pass security check.");
                    return;
                }

                ChangePassword(user, InputConfirmNewPassword);
            }
            catch (Exception)
            {
                Console.WriteLine("Unexpected error occurred with method UpdatePassword in ForgotPasswordViewModel.");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
