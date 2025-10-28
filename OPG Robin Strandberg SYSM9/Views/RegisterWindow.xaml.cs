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
            var username = UsernameBox.Text;
            var password = PasswordBox.Password;
            var country = CountryBox.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(country))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            _viewModel.CreateUser(username, password, country);
            MessageBox.Show("User registered successfully!");

            var main = new MainWindow();
            main.Show();
            Close();
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            Close();
        }
    }
}
