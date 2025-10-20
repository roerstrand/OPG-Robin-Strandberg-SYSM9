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
using OPG_Robin_Strandberg_SYSM9.Views;

namespace OPG_Robin_Strandberg_SYSM9
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private User _loggedIn;

        public User LoggedIn
        {
            get { return _loggedIn; }
            set
            {
                _loggedIn = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }

        public List<User> Users { get; set; } = new List<User>();

        public bool IsAuthenticated => LoggedIn != null;

        private object _currentView;

        public Object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ICommand ShowRegisterCommand { get; }
        public ICommand ShowAddRecipeCommand { get; }
        public ICommand ShowViewRecipeDetailsCommand { get; }
        public ICommand ShowViewRecipeListCommand { get; }

        public MainWindowViewModel()
        {
            ShowRegisterCommand = new RelayCommand(o => CurrentView = new RegisterWindow()); // OpenRegister här istället för egen metod
            ShowAddRecipeCommand = new RelayCommand(o => CurrentView = new AddRecipeListViewModel());
            ShowViewRecipeDetailsCommand = new RelayCommand(o => CurrentView = new RecipeDetailWindow());
            ShowViewRecipeListCommand = new RelayCommand(o => CurrentView = new RecipeListWindow());
        }

        public bool Login(string username, string password)
        {
            foreach (User u in Users)
            {
                if (u.UserName == username && u.Password == password)
                {
                    LoggedIn = u;
                    return true;
                }
            }

            MessageBox.Show("Warning! Wrong password or username.");
            return false;
        }


        public void Logout()
        {
            LoggedIn = null;
        }

        public event PropertyChangedEventHandlerß PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}