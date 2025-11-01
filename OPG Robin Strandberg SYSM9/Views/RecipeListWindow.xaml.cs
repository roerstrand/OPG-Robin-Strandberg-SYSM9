using System.Windows;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.ViewModels;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class RecipeListWindow : Window
    {
        private readonly RecipeManager _recipeManager;

        public RecipeListWindow()
        {
            InitializeComponent();
            _recipeManager = new RecipeManager();
            DataContext = new RecipeListViewModel(_recipeManager, App.UserManager);
        }

        public RecipeListWindow(RecipeManager recipeManager)
        {
            InitializeComponent();
            _recipeManager = recipeManager ?? new RecipeManager();
            DataContext = new RecipeListViewModel(_recipeManager, App.UserManager);
        }
    }
}
