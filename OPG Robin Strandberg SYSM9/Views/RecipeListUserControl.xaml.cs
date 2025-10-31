using System.Windows.Controls;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.ViewModels;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class RecipeListUserControl : UserControl
    {
        private readonly RecipeManager _recipeManager;

        public RecipeListUserControl()
        {
            InitializeComponent();
            _recipeManager = new RecipeManager();
            DataContext = new RecipeListViewModel(_recipeManager, App.UserManager);
        }

        // ÖVerladdad konstrukror när recipemanager skickas som argument
        public RecipeListUserControl(RecipeManager recipeManager) : this()
        {
            _recipeManager = recipeManager ?? new RecipeManager();
            DataContext = new RecipeListViewModel(_recipeManager, App.UserManager);
        }
    }
}
