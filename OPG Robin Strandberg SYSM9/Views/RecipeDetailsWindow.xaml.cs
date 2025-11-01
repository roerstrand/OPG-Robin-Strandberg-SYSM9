using System.Windows;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Models;
using OPG_Robin_Strandberg_SYSM9.ViewModels;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class RecipeDetailsWindow : Window
    {
        private readonly RecipeManager _recipeManager;

        public RecipeDetailsWindow(Recipe recipe, RecipeManager recipeManager)
        {
            InitializeComponent();
            _recipeManager = recipeManager;
            DataContext = new RecipeDetailViewModel(recipe, _recipeManager);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            var listWindow = new RecipeListWindow(_recipeManager);
            listWindow.Show();
            Close();
        }
    }
}
