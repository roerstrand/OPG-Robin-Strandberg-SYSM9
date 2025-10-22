using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OPG_Robin_Strandberg_SYSM9;

public class ForgotPasswordViewModel
{

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
