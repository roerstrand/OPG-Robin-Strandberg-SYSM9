using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Commands;

namespace OPG_Robin_Strandberg_SYSM9
{
    public class ForgotPasswordViewModel : INotifyPropertyChanged
    {
        private readonly UserManager _userManager;

        // tillfälliga inmatningsfält – används bara för formuläret
        private string _inputUsername;
        private string _inputNewPassword;
        private string _inputConfirmPassword;

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

        public string InputConfirmPassword
        {
            get => _inputConfirmPassword;
            set
            {
                _inputConfirmPassword = value;
                OnPropertyChanged();
            }
        }

        // kommando kopplat till knappen
        public RelayCommand SubmitCommand { get; }

        public ForgotPasswordViewModel()
        {
            _userManager = App.UserManager;
            SubmitCommand = new RelayCommand(o => ChangePassword());
        }

        private void ChangePassword()
        {
            if (string.IsNullOrWhiteSpace(InputUsername) ||
                string.IsNullOrWhiteSpace(InputNewPassword) ||
                string.IsNullOrWhiteSpace(InputConfirmPassword))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (InputNewPassword != InputConfirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            var foundUsers = _userManager.FindUser(InputUsername);
            if (foundUsers.Count == 0)
            {
                MessageBox.Show("User not found.");
                return;
            }

            // ändra lösenord för den användare som matchar exakt
            var user = foundUsers.FirstOrDefault(u => u.UserName == InputUsername);
            if (user != null)
            {
                user.Password = InputNewPassword;
                MessageBox.Show("Password successfully changed!");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
