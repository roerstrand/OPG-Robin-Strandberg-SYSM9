using System;
using System.Windows;
using System.Windows.Controls;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Models;
using OPG_Robin_Strandberg_SYSM9.ViewModels;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class AddRecipeWindow : Window
    {
        private readonly RecipeManager _recipeManager;

        public AddRecipeWindow(RecipeManager recipeManager)
        {
            InitializeComponent();
            _recipeManager = recipeManager ?? App.UserManager?.GetRecipeManagerForCurrentUser();
            DataContext = new AddRecipeViewModel(_recipeManager);
        }

        private void SaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = App.UserManager?.CurrentUser;
            if (currentUser == null)
            {
                MessageBox.Show("No user logged in.", "Session", MessageBoxButton.OK, MessageBoxImage.Warning);
                Close();
                return;
            }

            string title = titleInput.Text.Trim();
            string ingredients = ingredientsInput.Text.Trim();
            string category = categoryInput.Text.Trim();
            string instructions = instructionsInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(ingredients) ||
                string.IsNullOrWhiteSpace(category) ||
                string.IsNullOrWhiteSpace(instructions))
            {
                MessageBox.Show("All fields must be filled in.", "Missing info", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var newRecipe = new Recipe(title, instructions, category, DateTime.Now, currentUser, ingredients);
                _recipeManager.AddRecipe(newRecipe);

                MessageBox.Show("Recipe added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                var listWindow = new RecipeListWindow(_recipeManager);
                listWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while saving recipe: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackToList_Click(object sender, RoutedEventArgs e)
        {
            var listWindow = new RecipeListWindow(_recipeManager);
            listWindow.Show();
            Close();
        }
    }
}
