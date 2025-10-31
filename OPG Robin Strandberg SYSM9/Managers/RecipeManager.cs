﻿using System;
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
        private ObservableCollection<Recipe> _recipes;

        public ObservableCollection<Recipe> RecipeList
        {
            get => _recipes;
            private set
            {
                _recipes = value ?? new ObservableCollection<Recipe>();
                OnPropertyChanged();
            }
        }

        public RecipeManager()
        {
            RecipeList = new ObservableCollection<Recipe>();
        }

        public void AddRecipe(Recipe recipe)
        {
            try
            {
                if (recipe == null)
                {
                    MessageBox.Show("Recipe could not be added because it was empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (RecipeList.Any(r => string.Equals(r.Title, recipe.Title, StringComparison.OrdinalIgnoreCase)
                                        && r.CreatedBy?.UserName == recipe.CreatedBy?.UserName))
                {
                    MessageBox.Show("A recipe with that title already exists for this user.", "Duplicate", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                RecipeList.Add(recipe);
                OnPropertyChanged(nameof(RecipeList));
                MessageBox.Show($"Recipe \"{recipe.Title}\" was successfully added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    MessageBox.Show("No recipe selected to remove.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!RecipeList.Contains(recipe))
                {
                    MessageBox.Show("That recipe was not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                RecipeList.Remove(recipe);
                OnPropertyChanged(nameof(RecipeList));
                MessageBox.Show($"Recipe \"{recipe.Title}\" was removed.", "Removed", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    MessageBox.Show("User not found. Cannot load recipes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        public ObservableCollection<Recipe> Filter(string criteria)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(criteria))
                    return new ObservableCollection<Recipe>(RecipeList);

                criteria = criteria.Trim().ToLowerInvariant();

                var filtered = RecipeList
                    .Where(r => r.Title.ToLowerInvariant().Contains(criteria)
                             || r.Category.ToLowerInvariant().Contains(criteria))
                    .ToList();

                if (filtered.Count == 0)
                    MessageBox.Show("No recipes matched your search.", "No Results", MessageBoxButton.OK, MessageBoxImage.Information);

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
                    MessageBox.Show("No recipe selected to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(instructions))
                {
                    MessageBox.Show("Title and instructions must be filled in.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                recipe.Title = title;
                recipe.Instructions = instructions;
                recipe.Category = string.IsNullOrWhiteSpace(category) ? "Uncategorized" : category;

                recipe.Ingredients = ingredients?
                    .Split(',')
                    .Select(i => i.Trim())
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .ToList()
                    ?? new List<string>();

                OnPropertyChanged(nameof(RecipeList));
                MessageBox.Show($"Recipe \"{recipe.Title}\" was updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
