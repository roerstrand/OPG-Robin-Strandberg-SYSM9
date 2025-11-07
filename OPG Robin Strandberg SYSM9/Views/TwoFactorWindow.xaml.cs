using System.Windows;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class TwoFactorWindow : Window
    {
        private readonly string _expectedCode;
        private int _attemptsLeft = 3;

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
                _attemptsLeft--;
                CodeBox.Clear();
                CodeBox.Focus();

                if (_attemptsLeft > 0)
                {
                    MessageBox.Show($"Incorrect verification code. {_attemptsLeft} attempts remaining.",
                        "Invalid code", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show("Too many failed attempts. Login cancelled.",
                        "Access denied", MessageBoxButton.OK, MessageBoxImage.Error);

                    DialogResult = false;
                    Close();
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
