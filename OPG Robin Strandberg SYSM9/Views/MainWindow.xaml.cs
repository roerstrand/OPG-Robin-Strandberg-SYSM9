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
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
        }

        // Metod för att konvertera innehåll i vyns passwordbox till egenskapar som håller
        // värden för password och username i View Model.
        public void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
           _viewModel.PasswordInput = ((PasswordBox)sender).Password;
        }
    }

}
