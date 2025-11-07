using System.Collections.ObjectModel;
using System.Windows;
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
            _recipeManager = recipeManager ?? new RecipeManager(this);
        }

        public void RemoveAnyRecipe(Recipe recipe)
        {
            try
            {
                if (recipe == null)
                {
                    MessageBox.Show("No recipe selected to remove.",
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _recipeManager.RecipeList.Remove(recipe);
                OnPropertyChanged(nameof(RecipeList));

                MessageBox.Show($"Recipe \"{recipe.Title}\" was removed by administrator.",
                    "Removed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while removing recipe: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ObservableCollection<Recipe> ViewAllRecipes()
        {
            return _recipeManager.GetAllRecipes();
        }
    }
}
