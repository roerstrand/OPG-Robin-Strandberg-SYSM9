using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPG_Robin_Strandberg_SYSM9.Models;

namespace OPG_Robin_Strandberg_SYSM9.Managers
{
    internal class RecipeManager : INotifyPropertyChanged
    {
        private List<Recipe> _recipes;

        public List<Recipe> Recipes
        {
            get => _recipes;
            set => _recipes = value;
            OnPropertyChanged();
        }

        public RecipeManager(List<Recipe> recipes)
        {
            Recipes = recipes;
        }

        public void AddRecipe(Recipe recipe)
        {
            Recipes.Add(recipe);
        }

        public void RemoveRecipe(Recipe recipe)
        {
            Recipes.Remove(recipe);
        }

        public Recipe GetAllRecipes()
        {
            return Recipes;
        }

        public User GetByUser(User user)
        {
            return user;
        }

        public Recipe Filter(string criteria) // filtrering string titel eller kategori
        {
            foreach (recipe Recipe in Recipes)
            {
                if (criteria == recipe.Title || criteria == recipe.Category)
                {
                    return recipe;
                }
            }

            return Recipes;
        }

        public void UpdateRecipe(string title, string instructions, string category, DateTime createdAt, User createdBy,
            List<string, string, string> ingredients)
        {
            Title = title;
            Instructions = instructions;
            Category = category;
            CreatedAt = createdAt;
            CreatedBy = createdBy;

            _ingredients = new List<string>(ingredients);
        }
    }
}