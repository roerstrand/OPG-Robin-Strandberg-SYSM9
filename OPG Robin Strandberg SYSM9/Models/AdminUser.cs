using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Models;

namespace OPG_Robin_Strandberg_SYSM9.Models
{
    public class AdminUser : User, INotifyPropertyChanged
    {
        private readonly RecipeManager _recipeManager;

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
