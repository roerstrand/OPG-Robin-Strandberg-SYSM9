using System.ComponentModel;
using System.Runtime.CompilerServices;
using OPG_Robin_Strandberg_SYSM9.Managers;

namespace OPG_Robin_Strandberg_SYSM9;

public class ForgotPasswordViewModel
{

    private readonly UserManager _userManager;

    public ForgotPasswordViewModel()
    {
        _userManager = App.UserManager;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
