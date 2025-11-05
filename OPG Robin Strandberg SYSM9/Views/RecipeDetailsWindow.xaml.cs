using System.Windows;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Models;
using OPG_Robin_Strandberg_SYSM9.ViewModels;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class RecipeDetailsWindow : Window
    {
        private readonly RecipeManager _recipeManager;

        private readonly RecipeDetailViewModel _viewModel;

        public RecipeDetailsWindow(Recipe recipe, RecipeManager recipeManager)
        {
            InitializeComponent();
            _recipeManager = recipeManager;

            _viewModel = new RecipeDetailViewModel(recipe, _recipeManager);
            DataContext = _viewModel;
        }
    }
}
