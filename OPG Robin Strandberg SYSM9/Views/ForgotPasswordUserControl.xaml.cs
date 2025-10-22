using System.Windows.Controls;

namespace OPG_Robin_Strandberg_SYSM9.Views;

public partial class ForgotPasswordUserControl : UserControl
{
    public ForgotPasswordUserControl()
    {
        InitializeComponent();
        DataContext = new ForgotPasswordViewModel();
    }
}

