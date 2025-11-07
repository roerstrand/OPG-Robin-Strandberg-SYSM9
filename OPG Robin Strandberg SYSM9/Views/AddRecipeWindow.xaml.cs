using System;
using System.Windows;
using System.Windows.Controls;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Models;
using OPG_Robin_Strandberg_SYSM9.ViewModels;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class AddRecipeWindow : Window
    {
        private readonly RecipeManager _recipeManager;

        public AddRecipeWindow(RecipeManager recipeManager)
        {
            InitializeComponent();
            _recipeManager = recipeManager ?? App.UserManager?.GetRecipeManagerForCurrentUser();
            DataContext = new AddRecipeViewModel(_recipeManager);
        }
    }
}
