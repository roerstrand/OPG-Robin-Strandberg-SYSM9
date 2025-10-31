using System;
using System.Windows;
using System.Windows.Controls;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Models;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class AddRecipeUserControl : UserControl
    {
        private readonly RecipeManager _recipeManager;

        public AddRecipeUserControl()
        {
            InitializeComponent();
            _recipeManager = new RecipeManager();
        }

        public AddRecipeUserControl(RecipeManager recipeManager)
        {
            InitializeComponent();
            _recipeManager = recipeManager;
        }

        private void SaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = App.UserManager?.CurrentUser;
            if (currentUser == null)
            {
                MessageBox.Show("Ingen användare är inloggad. Återgår till startsidan.",
                                "Session",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);

                Application.Current.MainWindow.Content = new RecipeListUserControl(_recipeManager);
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
                MessageBox.Show("All fields must be filled in.",
                                "Missing information",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            try
            {
                var newRecipe = new Recipe(
                    title,
                    instructions,
                    category,
                    DateTime.Now,
                    currentUser,
                    ingredients
                );

                _recipeManager.AddRecipe(newRecipe);

                MessageBox.Show("Recipe added successfully!",
                                "Success",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                titleInput.Clear();
                ingredientsInput.Clear();
                categoryInput.Clear();
                instructionsInput.Clear();

                Application.Current.MainWindow.Content = new RecipeListUserControl(_recipeManager);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while saving recipe: {ex.Message}",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void BackToList_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new RecipeListUserControl(_recipeManager);
        }
    }
}
