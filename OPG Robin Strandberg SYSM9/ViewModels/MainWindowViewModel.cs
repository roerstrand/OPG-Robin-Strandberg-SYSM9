using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Models;
using OPG_Robin_Strandberg_SYSM9.Views;
using OPG_Robin_Strandberg_SYSM9.Commands;

namespace OPG_Robin_Strandberg_SYSM9
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly UserManager _userManager;

        private string _userNameInput;

        public string UserNameInput
        {
            get => _userNameInput;
            set
            {
                _userNameInput = value;
                OnPropertyChanged();
            }
        }

        private string _passwordInput;

        public string PasswordInput
        {
            get => _passwordInput;
            set { _passwordInput = value; }
        }

        private User _loggedIn;

        public User LoggedIn
        {
            get { return _loggedIn; }
            set
            {
                _loggedIn = value;
                OnPropertyChanged();
            }
        }

        public List<User> Users { get; set; } = new List<User>();

        public bool IsAuthenticated => _userManager.IsAuthenticated;

        private object _currentView;

        public Object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        // Relay commands som skapar ny instanser av fönster som sedan inbäddas i MainContentSection
        // i main window.

        public ICommand ShowRegisterCommand { get; }
        public ICommand ShowForgotPasswordCommand { get; }
        public ICommand ShowAddRecipeCommand { get; }
        public ICommand ShowViewRecipeDetailsCommand { get; }
        public ICommand ShowViewRecipeListCommand { get; }

        public ICommand ShowViewUserDetailsCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainWindowViewModel()
        {
            // koppla lokala egenskaper till globala statiska instantierade objekt

            _userManager = App.UserManager;

            // Loginsection innan lyckad inloggning

            LoginCommand = new RelayCommand(o => Login_Button());
            ShowRegisterCommand = new RelayCommand(o =>
            {
                var register = new RegisterWindow();
                register.Show();
                Application.Current.MainWindow.Close();
            });

            ShowForgotPasswordCommand = new RelayCommand(o =>
            {
                var forgot = new ForgotPasswordWindow();
                forgot.Show();
                Application.Current.MainWindow.Close();
            });


            // Maincontentsection efter inlogging

            ShowAddRecipeCommand = new RelayCommand(o => CurrentView = new AddRecipeWindow());
            ShowViewRecipeDetailsCommand = new RelayCommand(o => CurrentView = new RecipeDetailWindow());
            ShowViewRecipeListCommand = new RelayCommand(o => CurrentView = new RecipeListWindow());
            ShowViewUserDetailsCommand = new RelayCommand(o => CurrentView = new UserDetailsWindow());
            LogoutCommand = new RelayCommand(o => Logout_Button());
        }

        public void Login_Button()
        {
            _userManager.Login(UserNameInput, PasswordInput);
        }

        public void Logout_Button()
        {
            _userManager.Logout();
            OnPropertyChanged(nameof(PasswordInput));
            UserNameInput = string.Empty;
            PasswordInput = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
