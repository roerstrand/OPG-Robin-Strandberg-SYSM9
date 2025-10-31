using System.Windows;
using OPG_Robin_Strandberg_SYSM9.ViewModels;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly RegisterViewModel _viewModel;

        public RegisterWindow()
        {
            InitializeComponent();
            _viewModel = (RegisterViewModel)DataContext;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameBox.Text.Trim();
            var password = PasswordBox.Password.Trim();
            var country = CountryBox.SelectedItem?.ToString();

            bool success = _viewModel.CreateUser(username, password, country);

            if (success)
            {
                this.Close();
            }
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
           this.Close();
        }
    }
}
