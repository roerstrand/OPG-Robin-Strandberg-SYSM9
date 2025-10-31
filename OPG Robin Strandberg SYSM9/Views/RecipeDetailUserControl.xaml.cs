using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Models;
using OPG_Robin_Strandberg_SYSM9.ViewModels;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class RecipeDetailUserControl : UserControl
    {
        private readonly RecipeManager _recipeManager;

        public RecipeDetailUserControl(Recipe recipe, RecipeManager recipeManager)
        {
            InitializeComponent();
            _recipeManager = recipeManager;
            DataContext = new RecipeDetailViewModel(recipe, _recipeManager);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            MediaTypeNames.Application.Current.MainWindow.Content = new RecipeListUserControl(_recipeManager);
        }
    }
}
