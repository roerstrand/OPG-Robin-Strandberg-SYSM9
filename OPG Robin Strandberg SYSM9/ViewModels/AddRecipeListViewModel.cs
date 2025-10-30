using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace OPG_Robin_Strandberg_SYSM9;

public class AddRecipeListViewModel
{
    public void Add_RecipeButton(object sender, RoutedEventArgs e)
    {

    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
