using System.Windows;
using System.Windows.Controls;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.ViewModels;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class AddRecipeUserControl : UserControl
    {
        private readonly RecipeManager _recipeManager;

        public AddRecipeUserControl(RecipeManager recipeManager)
        {
            InitializeComponent();
            _recipeManager = recipeManager;
            DataContext = new AddRecipeViewModel(_recipeManager);
        }
    }
}
