using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Commands;
using OPG_Robin_Strandberg_SYSM9.Models;
using OPG_Robin_Strandberg_SYSM9.Views;

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

        private int Attempts
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
            // App.UserManager global objektinstans tilldelas i konstruktorn app.xaml vid kompilering app-klass
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
                    MessageBox.Show("User not found", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var lastFour = user.Password.Substring(user.Password.Length - 4).Trim();

                if (lastFour != LastFourPreviousPassword)
                {
                    Attempts++;
                    if (Attempts >= 3)
                    {
                        CanSubmit = false;
                        MessageBox.Show("Too many failed attempts. Please try again later.",
                            "Access denied", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    int remaining = 3 - Attempts;
                    MessageBox.Show($"Security check failed. {remaining} attempts remaining.",
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (InputNewPassword != InputConfirmNewPassword)
                {
                    MessageBox.Show("Passwords do not match.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Stark validering (samma som vid registrering)
                string pattern = @"^(?=.*\d)(?=.*[!@#$%^&*(),.?""':{}|<>])[A-Za-z\d!@#$%^&*(),.?""':{}|<>]{8,}$";
                if (!Regex.IsMatch(InputNewPassword, pattern))
                {
                    MessageBox.Show(
                        "Password must be at least 8 characters long and include at least one digit and one special character.",
                        "Invalid Password",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                bool success = _userManager.ChangePassword(user, InputNewPassword);

                if (success)
                {
                    MessageBox.Show("Password successfully changed. You can now log in with your new password.",
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                foreach (Window window in Application.Current.Windows)
                {
                    if (window is Views.ForgotPasswordWindow forgotWindow)
                    {
                        var mainWindow = new MainWindow();
                        mainWindow.Show();
                        forgotWindow.Close();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
                        var mainWindow = new MainWindow();
                        mainWindow.Show();
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

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
