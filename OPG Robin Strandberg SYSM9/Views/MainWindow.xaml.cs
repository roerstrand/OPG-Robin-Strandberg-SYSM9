using System.ComponentModel;
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

namespace OPG_Robin_Strandberg_SYSM9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        private bool _isUpdatingPasswordFromVM = false;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

            // Läggs t nedan metod att lyssna
            // på PropertyChanged-signal från MainWindowViewModel. OneWay VM till vy, för att kunna
            // tömma passwordbox vid utloggning

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;

        }

        // Metod för att hämta innehåll från vyns passwordbox
        //_isUpdatingPasswordFromVM används för att skicka signaler one way till VM om inte
        //PasswordBox updateras/töms av nedan metod ViewModel_PropertyChanged.
        public void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!_isUpdatingPasswordFromVM)
            {
                _viewModel.PasswordInput = ((PasswordBox)sender).Password;
            }
        }

        public void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowViewModel.PasswordInput))
            {
                // Skicka asynkron dispatch-signal till UI-tråden att uppdatera PasswordBox
                // vid ändring av motsvarande egenskap i VM
                _isUpdatingPasswordFromVM = true;
                Dispatcher.Invoke(() => MainWindowPasswordBox.Password = string.Empty);
                _isUpdatingPasswordFromVM = false;
            }
        }

    }

}
