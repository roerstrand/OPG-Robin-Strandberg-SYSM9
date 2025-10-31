using System.ComponentModel;
using System.Diagnostics;
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

namespace OPG_Robin_Strandberg_SYSM9.Views
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

            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

            InitializeComponent();

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;

        }

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
                _isUpdatingPasswordFromVM = true;
                Dispatcher.Invoke(() => MainWindowPasswordBox.Password = string.Empty);
                _isUpdatingPasswordFromVM = false;
            }
        }

    }

}
