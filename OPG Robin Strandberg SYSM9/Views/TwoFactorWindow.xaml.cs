using System.Windows;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class TwoFactorWindow : Window
    {
        private readonly string _expectedCode;

        public TwoFactorWindow(string expectedCode)
        {
            InitializeComponent();
            _expectedCode = expectedCode;
        }

        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            if (CodeBox.Text == _expectedCode)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Incorrect verification code. Please try again.",
                    "Invalid code", MessageBoxButton.OK, MessageBoxImage.Warning);
                CodeBox.Clear();
                CodeBox.Focus();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
