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

namespace OPG_Robin_Strandberg_SYSM9
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public string UserNameInput;
        public string PasswordInput;

        private User _loggedIn;

        public User LoggedIn
        {
            get { return _loggedIn; }
            set
            {
                _loggedIn = value;
                OnPropertyChanged(nameof(LoggedIn));
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }

        public List<User> Users { get; set; } = new List<User>();

        public bool IsAuthenticated = false;

        public object _currentView;

        public Object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
            }
        }

        UserManager _userManager = new UserManager(); // En instans av user manager skapas direkt,
                                                      // som i konstruktorn skapar en lista med nya default users
                                                      private RecipeManager _recipeManager = new RecipeManager(List<Recipe> recipes);

        // Relay commands som skapar ny instanser av fönster som seddan inbäddas i MainContentSection
        // i min window.

        public ICommand ShowRegisterCommand { get; }
        public ICommand ShowForgotPasswordCommand { get; }
        public ICommand ShowAddRecipeCommand { get; }
        public ICommand ShowViewRecipeDetailsCommand { get; }
        public ICommand ShowViewRecipeListCommand { get; }
        public ICommand LoginCommand { get; }

        public MainWindowViewModel()
        {
            ShowRegisterCommand = new RelayCommand(o => CurrentView = new RegisterUserControl()); // OpenRegister här istället för egen metod
            ShowAddRecipeCommand = new RelayCommand(o => CurrentView = new AddRecipeListViewModel());
            ShowViewRecipeDetailsCommand = new RelayCommand(o => CurrentView = new RecipeDetailUserControl());
            ShowViewRecipeListCommand = new RelayCommand(o => CurrentView = new RecipeListUserControl());
            ShowForgotPasswordCommand = new RelayCommand(o => CurrentView = new ForgotPasswordUserControl());

            LoginCommand = new RelayCommand(o => Login_Button());
        }

        public void Login_Button()
        {
            foreach (User u in _userManager.Users)
            {
                if (u.UserName == UserNameInput && u.Password == PasswordInput)
                {
                    LoggedIn = u;
                    IsAuthenticated = true;
                    MessageBox.Show("Welcome {u.UserName}!");
                }
            }

            MessageBox.Show("Warning! Wrong password or username.");
        }

        public void Logout_Button(object sender, RoutedEventArgs e)
        {
            LoggedIn = null;
            IsAuthenticated = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
