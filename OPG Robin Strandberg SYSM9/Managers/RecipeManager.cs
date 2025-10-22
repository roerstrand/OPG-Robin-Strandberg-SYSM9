
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using OPG_Robin_Strandberg_SYSM9.Models;

namespace OPG_Robin_Strandberg_SYSM9.Managers
{
    internal class RecipeManager : INotifyPropertyChanged
    {
        private List<Recipe> _recipes;

        public List<Recipe> RecipeList
        {
            get => _recipes;
            set
            {
                _recipes = value;
                OnPropertyChanged(RecipeList.ToString());
            }
        }

        public RecipeManager(List<Recipe> recipes)
        {
            RecipeList = recipes;
        }

        public void AddRecipe(Recipe recipe)
        {
            RecipeList.Add(recipe);
        }

        public void RemoveRecipe(Recipe recipe)
        {
            RecipeList.Remove(recipe);
        }

        public List<Recipe> GetAllRecipes()
        {
            return RecipeList;
        }

        public List<Recipe> GetByUser(User UserName)
        {
            List<Recipe> RecipesByUser = new List<Recipe>();

            foreach (Recipe recipe in RecipeList)
            {
                RecipesByUser.Add(recipe);
            }

            return RecipesByUser;
        }

        public List<Recipe> Filter(string criteria) // filtrering string titel eller kategori
        {
            List<Recipe> FilteredRecipes = new List<Recipe>();

            foreach (Recipe recipe in RecipeList)
            {
                if (criteria == recipe.Title || criteria == recipe.Category)
                {
                    FilteredRecipes.Add(recipe);
                }
            }

            return FilteredRecipes;
        }

        public void UpdateRecipe(string title, string instructions, string category, DateTime createdAt, User createdBy,
            string ingredients)
        {

        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
