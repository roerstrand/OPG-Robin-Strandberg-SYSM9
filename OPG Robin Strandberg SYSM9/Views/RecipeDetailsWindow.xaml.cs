using System.Windows;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Models;
using OPG_Robin_Strandberg_SYSM9.ViewModels;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class RecipeDetailsWindow : Window
    {
        private readonly RecipeManager _recipeManager;

        private readonly RecipeDetailsViewModel _viewModel;

        public RecipeDetailsWindow(Recipe recipe, RecipeManager recipeManager)
        {
            InitializeComponent();
            _recipeManager = recipeManager ?? App.UserManager?.GetRecipeManagerForCurrentUser();
            DataContext = new RecipeDetailsViewModel(recipe,  _recipeManager);
        }
    }
}
