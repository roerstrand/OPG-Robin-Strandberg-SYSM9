using System.Collections.ObjectModel;
using OPG_Robin_Strandberg_SYSM9.Managers;

namespace OPG_Robin_Strandberg_SYSM9.Models
{
    public class AdminUser : User
    {
        private readonly RecipeManager _recipeManager;

        public override bool IsAdmin => true;

        public AdminUser(string username, string password, string country, RecipeManager recipeManager = null)
            : base(username, password, country)
        {
            _recipeManager = recipeManager ?? new RecipeManager();
        }

        public void RemoveAnyRecipe(Recipe recipe)
        {
            if (recipe == null) return;

            _recipeManager.RemoveRecipe(recipe);
            OnPropertyChanged(nameof(ViewAllRecipes));
        }

        public ObservableCollection<Recipe> ViewAllRecipes()
        {
            return _recipeManager.GetAllRecipes();
        }
    }
}
