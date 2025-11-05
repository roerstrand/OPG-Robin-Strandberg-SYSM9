using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using OPG_Robin_Strandberg_SYSM9.Models;

namespace OPG_Robin_Strandberg_SYSM9.Managers
{
    public class RecipeManager : INotifyPropertyChanged
    {
        private Recipe _currentRecipe;

        public Recipe CurrentRecipe
        {
            get => _currentRecipe;
            set
            {
                if (_currentRecipe != value)
                {
                    _currentRecipe = value;
                    OnPropertyChanged(nameof(CurrentRecipe));
                }
            }
        }

        private ObservableCollection<Recipe> _recipes;

        public ObservableCollection<Recipe> RecipeList
        {
            get => _recipes;
            private set
            {
                _recipes = value;
                OnPropertyChanged();
            }
        }

        public RecipeManager()
        {
            RecipeList = new ObservableCollection<Recipe>();
            RecipeList.Add(new Recipe(
                "Classic Pancakes",
                "Mix flour, milk, eggs, and butter. Fry in pan until golden.",
                "Breakfast",
                DateTime.Now,
                new User("System", "default1", "Sweden"),
                "Flour, Milk, Eggs, Butter, Salt"
            ));

            RecipeList.Add(new Recipe(
                "Spaghetti Bolognese",
                "Cook pasta. Prepare sauce with minced meat, tomatoes, and herbs. Combine and serve.",
                "Dinner",
                DateTime.Now,
                new User("System", "default2", "Italy"),
                "Spaghetti, Minced Meat, Tomato Sauce, Garlic, Onion, Herbs"
            ));

            RecipeList.Add(new Recipe(
                "Chocolate Chip Cookies",
                "Mix dough with butter, sugar, eggs, and chocolate chips. Bake until golden brown.",
                "Dessert",
                DateTime.Now,
                new User("System", "default3", "USA"),
                "Butter, Sugar, Eggs, Flour, Chocolate Chips"
            ));
        }

        public void AddRecipe(Recipe recipe)
        {
            try
            {
                if (recipe == null)
                {
                    MessageBox.Show("Recipe could not be added because it was empty.", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                if (RecipeList.Any(r => string.Equals(r.Title, recipe.Title, StringComparison.Ordinal)
                                        && r.CreatedBy?.UserName == recipe.CreatedBy?.UserName))
                {
                    MessageBox.Show("A recipe with that title already exists for this user.", "Duplicate",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                RecipeList.Add(recipe);
                OnPropertyChanged(nameof(RecipeList));
                MessageBox.Show($"Recipe \"{recipe.Title}\" was successfully added!", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ShowError("An unexpected error occurred while adding the recipe.", ex);
            }
        }

        public void RemoveRecipe(Recipe recipe)
        {
            try
            {
                if (recipe == null)
                {
                    MessageBox.Show("No recipe selected to remove.", "Warning", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                if (!RecipeList.Contains(recipe))
                {
                    MessageBox.Show("That recipe was not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                RecipeList.Remove(recipe);
                OnPropertyChanged(nameof(RecipeList));
                MessageBox.Show($"Recipe \"{recipe.Title}\" was removed.", "Removed", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ShowError("An unexpected error occurred while removing the recipe.", ex);
            }
        }

        public ObservableCollection<Recipe> GetAllRecipes()
        {
            try
            {
                return new ObservableCollection<Recipe>(RecipeList);
            }
            catch (Exception ex)
            {
                ShowError("Unable to load recipes.", ex);
                return new ObservableCollection<Recipe>();
            }
        }

        public List<Recipe> GetByUser(User user)
        {
            try
            {
                if (user == null)
                {
                    MessageBox.Show("User not found. Cannot load recipes.", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return new List<Recipe>();
                }

                return RecipeList
                    .Where(r => r.CreatedBy?.UserName == user.UserName)
                    .ToList();
            }
            catch (Exception ex)
            {
                ShowError("Unable to load recipes for this user.", ex);
                return new List<Recipe>();
            }
        }

        public ObservableCollection<Recipe> ViewAllRecipes(User currentUser, List<User> activeAdmins)
        {
            try
            {
                if (currentUser == null)
                {
                    MessageBox.Show("No user is logged in. Cannot load recipes.",
                        "Session", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return new ObservableCollection<Recipe>();
                }

                if (activeAdmins != null && activeAdmins.Contains(currentUser))
                {
                    return new ObservableCollection<Recipe>(RecipeList);
                }

                return new ObservableCollection<Recipe>(
                    RecipeList.Where(r => r.CreatedBy?.UserName == currentUser.UserName)
                );
            }
            catch (Exception ex)
            {
                ShowError("Unexpected error when loading recipes.", ex);
                return new ObservableCollection<Recipe>();
            }
        }

        public ObservableCollection<Recipe> GetAllRecipesForAdmin(List<User> allUsers)
        {
            try
            {
                var allRecipes = new ObservableCollection<Recipe>();

                if (allUsers == null || allUsers.Count == 0)
                    return allRecipes;

                foreach (var user in allUsers)
                {
                    var userRecipes = GetByUser(user);
                    foreach (var recipe in userRecipes)
                    {
                        if (!allRecipes.Contains(recipe))
                            allRecipes.Add(recipe);
                    }
                }

                return allRecipes;
            }
            catch (Exception ex)
            {
                ShowError("Unexpected error while loading all recipes for admin.", ex);
                return new ObservableCollection<Recipe>();
            }
        }


        public ObservableCollection<Recipe> Filter(string criteria)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(criteria))
                    return new ObservableCollection<Recipe>(RecipeList);

                criteria = criteria.Trim();

                var filtered = RecipeList
                    .Where(r =>
                        r.Title.Contains(criteria, StringComparison.Ordinal) ||
                        r.Category.Contains(criteria, StringComparison.Ordinal))
                    .ToList();

                if (filtered.Count == 0)
                    MessageBox.Show("No recipes matched your search.", "No Results", MessageBoxButton.OK,
                        MessageBoxImage.Information);

                return new ObservableCollection<Recipe>(filtered);
            }
            catch (Exception ex)
            {
                ShowError("An error occurred while filtering recipes.", ex);
                return new ObservableCollection<Recipe>();
            }
        }

        public void UpdateRecipe(Recipe recipe, string title, string instructions, string category, string ingredients)
        {
            try
            {
                if (recipe == null)
                {
                    MessageBox.Show("No recipe selected to update.", "Warning", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(instructions))
                {
                    MessageBox.Show("Title and instructions must be filled in.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                recipe.Title = title;
                recipe.Instructions = instructions;
                recipe.Category = string.IsNullOrWhiteSpace(category) ? "Uncategorized" : category;

                List<string> ingredientsList;


                if (string.IsNullOrWhiteSpace(ingredients))
                {
                    ingredientsList = new List<string>();
                }
                else
                {
                    string[] parts = ingredients.Split(',');


                    ingredientsList = new List<string>();
                    foreach (string part in parts)
                    {
                        string trimmed = part.Trim();
                        if (!string.IsNullOrWhiteSpace(trimmed))
                        {
                            ingredientsList.Add(trimmed);
                        }
                    }
                }

                recipe.Ingredients = ingredientsList;

                OnPropertyChanged(nameof(RecipeList));
                MessageBox.Show($"Recipe \"{recipe.Title}\" was updated successfully.", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ShowError("An error occurred while updating the recipe.", ex);
            }
        }

        private void ShowError(string message, Exception ex)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] RecipeManager error: {ex.Message}");
            MessageBox.Show($"{message}\n\nDetails: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
