using System.Windows;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class ForgotPasswordWindow : Window
    {
        private readonly ForgotPasswordViewModel _viewModel;

        public ForgotPasswordWindow()
        {
            InitializeComponent();
            _viewModel = (ForgotPasswordViewModel)DataContext;
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.InputNewPassword = NewPasswordBox.Password;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.InputConfirmNewPassword = ConfirmNewPasswordBox.Password;
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LastFourPreviousPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.LastFourPreviousPassword = LastFourPreviousPassword.Password;
        }
    }
}
