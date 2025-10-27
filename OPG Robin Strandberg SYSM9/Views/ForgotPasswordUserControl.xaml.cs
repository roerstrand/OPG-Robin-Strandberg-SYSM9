using System.Windows.Controls;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class ForgotPasswordUserControl : UserControl
    {
        private readonly ForgotPasswordViewModel _viewModel;

        public ForgotPasswordUserControl()
        {
            InitializeComponent();
            _viewModel = new ForgotPasswordViewModel();
            DataContext = _viewModel;
        }

        private void NewPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.InputNewPassword = ((PasswordBox)sender).Password;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.InputConfirmPassword = ((PasswordBox)sender).Password;
        }
    }
}
