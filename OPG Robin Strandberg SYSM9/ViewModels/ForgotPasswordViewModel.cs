using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Commands;
using OPG_Robin_Strandberg_SYSM9.Models;

namespace OPG_Robin_Strandberg_SYSM9
{
    public class ForgotPasswordViewModel : INotifyPropertyChanged
    {
        private readonly UserManager _userManager;

        private string _inputUsername;
        private string _inputNewPassword;
        private string _inputConfirmPassword;
        private string _lastFourPreviousPassword;
        private int _attempts;
        private bool _canSubmit = true;

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

        public ICommand SubmitCommand { get; }
        public ICommand BackToLoginCommand { get; }

        public ForgotPasswordViewModel()
        {
            _userManager = App.UserManager;

            SubmitCommand = new RelayCommand(o => ValidatePriorPasswordChange());
            BackToLoginCommand = new RelayCommand(o => BackToLogin());
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
                var lastFour = password.Substring(password.Length - 4).Trim();

                if (lastFour != LastFourPreviousPassword)
                {
                    Attempts++;
                    if (Attempts >= 3)
                    {
                        CanSubmit = false;
                        return;
                    }

                    int remaining = 3 - Attempts;
                    MessageBox.Show($"Security check failed. {remaining} attempts remaining.");
                    return;
                }

                ChangePassword(user, InputConfirmNewPassword);
            }
            catch (Exception)
            {
                Console.WriteLine("Unexpected error in method ValidatePriorPasswordChange (ForgotPasswordViewModel)");
                MessageBox.Show("Unexpected error.");
            }
        }

        private void ChangePassword(User user, string newPassword)
        {
            try
            {
                var currentUser = _userManager.FindUser(user.UserName);

                if (currentUser == null)
                {
                    MessageBox.Show("User not found");
                    return;
                }

                if (InputNewPassword != newPassword)
                {
                    MessageBox.Show("Passwords do not match");
                    return;
                }

                _userManager.ChangePassword(user, newPassword);
            }
            catch (Exception)
            {
                Console.WriteLine("Unexpected error in ChangePassword (ForgotPasswordViewModel)");
                MessageBox.Show("Unexpected error.");
            }
        }

        private void BackToLogin()
        {
            try
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is Views.ForgotPasswordWindow forgotWindow)
                    {
                        forgotWindow.Close();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BackToLogin method in forgotpasswordviewmodel: {ex.Message}");
                MessageBox.Show("Unexpected error.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
