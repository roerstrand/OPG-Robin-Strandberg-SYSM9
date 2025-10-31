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
using OPG_Robin_Strandberg_SYSM9.ViewModels;

namespace OPG_Robin_Strandberg_SYSM9
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly UserManager _userManager;
        private RecipeManager _recipeManager;

        private string _userNameInput;

        private RecipeListViewModel _recipeListViewModel;
        public RecipeListViewModel RecipeListViewModel
        {
            get => _recipeListViewModel;
            set { _recipeListViewModel = value; OnPropertyChanged(); }
        }


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
            // kopplatill globala statisk user manager

            _userManager = App.UserManager;
            _recipeManager =
                new RecipeManager(); // null innan inloggning. En instans skapas vid lyckad inloggning Login_Button

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

            // Maincontentsection efter inloggning
// Maincontentsection efter inloggning
            ShowAddRecipeCommand = new RelayCommand(_ =>
            {
                Application.Current.MainWindow.Content = new AddRecipeUserControl(_recipeManager);
            });

            ShowViewRecipeListCommand = new RelayCommand(_ =>
            {
                Application.Current.MainWindow.Content = new RecipeListUserControl(_recipeManager);
            });

            ShowViewRecipeDetailsCommand = new RelayCommand(_ =>
            {
                // kontrollera att något recept är valt
                if (_recipeManager.CurrentRecipe == null)
                {
                    MessageBox.Show("Välj ett recept först.", "Information", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                Application.Current.MainWindow.Content =
                    new RecipeDetailsUserControl(_recipeManager.CurrentRecipe, _recipeManager);
            });

            LogoutCommand = new RelayCommand(_ => Logout_Button());
        }


        public void Login_Button()
        {
            if (_userManager.Login(UserNameInput, PasswordInput))
            {
                _recipeManager = _userManager.GetRecipeManagerForCurrentUser();
                RecipeListViewModel = new RecipeListViewModel(_recipeManager, _userManager);

                // Denna rad triggar din XAML så login-sektionen döljs och MainContent visas
                OnPropertyChanged(nameof(IsAuthenticated));
            }
            else
            {
                MessageBox.Show("Felaktigt användarnamn eller lösenord.",
                    "Inloggning misslyckades",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
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
