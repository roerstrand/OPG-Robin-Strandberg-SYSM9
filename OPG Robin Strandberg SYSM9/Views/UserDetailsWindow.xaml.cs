using System.Windows;
using System.Windows.Controls;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.ViewModels;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class UserDetailsWindow : Window
    {
        private readonly UserManager _userManager;

        public UserDetailsWindow(UserManager userManager)
        {
            InitializeComponent();
            _userManager = userManager;
            DataContext = new UserDetailsViewModel(_userManager);
        }

        private void PasswordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserDetailsViewModel vm)
                vm.NewPassword = ((PasswordBox)sender).Password;
        }

        private void PasswordBox2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserDetailsViewModel vm)
                vm.ConfirmPassword = ((PasswordBox)sender).Password;
        }
    }
}
