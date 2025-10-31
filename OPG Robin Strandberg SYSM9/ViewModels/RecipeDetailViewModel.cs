using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using OPG_Robin_Strandberg_SYSM9.Models;
using OPG_Robin_Strandberg_SYSM9.Views;
using OPG_Robin_Strandberg_SYSM9.Commands;
using OPG_Robin_Strandberg_SYSM9.Managers;

namespace OPG_Robin_Strandberg_SYSM9.ViewModels
{
    public class RecipeDetailViewModel : INotifyPropertyChanged
    {
        private readonly RecipeManager _recipeManager;
        private Recipe _recipe;
        private bool _isEditing;

        public Recipe Recipe
        {
            get => _recipe;
            set { _recipe = value; OnPropertyChanged(nameof(Recipe)); }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set { _isEditing = value; OnPropertyChanged(nameof(IsEditing)); OnPropertyChanged(nameof(IsReadOnly)); }
        }

        public bool IsReadOnly => !IsEditing;

        public string Title
        {
            get => Recipe.Title;
            set { Recipe.Title = value; OnPropertyChanged(nameof(Title)); }
        }

        public string Ingredients
        {
            get => string.Join(", ", Recipe.Ingredients);
            set
            {
                Recipe.Ingredients = value.Split(',').Select(i => i.Trim()).ToList();
                OnPropertyChanged(nameof(Ingredients));
            }
        }

        public string Instructions
        {
            get => Recipe.Instructions;
            set { Recipe.Instructions = value; OnPropertyChanged(nameof(Instructions)); }
        }

        public string Category
        {
            get => Recipe.Category;
            set { Recipe.Category = value; OnPropertyChanged(nameof(Category)); }
        }

        public DateTime CreatedAt
        {
            get => Recipe.CreatedAt;
            set { Recipe.CreatedAt = value; OnPropertyChanged(nameof(CreatedAt)); }
        }

        public string CreatedBy => Recipe.CreatedBy.UserName;

        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CopyCommand { get; }
        public ICommand CancelCommand { get; }

        public RecipeDetailViewModel(Recipe recipe, RecipeManager recipeManager)
        {
            Recipe = recipe;
            _recipeManager = recipeManager;
            IsEditing = false;

            EditCommand = new RelayCommand(_ => IsEditing = true);
            SaveCommand = new RelayCommand(_ => SaveRecipe());
            CopyCommand = new RelayCommand(_ => CopyRecipe());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private void SaveRecipe()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Title) ||
                    string.IsNullOrWhiteSpace(Instructions) ||
                    string.IsNullOrWhiteSpace(Category) ||
                    Recipe.Ingredients == null || !Recipe.Ingredients.Any())
                {
                    MessageBox.Show("All fields must be filled in.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Uppdatera befintligt recept i RecipeManager
                var existingRecipe = _recipeManager.RecipeList.FirstOrDefault(r => r.Title == Recipe.Title);
                if (existingRecipe != null)
                {
                    existingRecipe.EditRecipe(Title, Instructions, Category, DateTime.Now, Recipe.CreatedBy, string.Join(", ", Recipe.Ingredients));
                }

                MessageBox.Show("Recipe saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                Application.Current.MainWindow.Content = new Views.RecipeListUserControl(_recipeManager);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving recipe: {ex.Message}", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CopyRecipe()
        {
            try
            {
                var copiedRecipe = new Recipe(
                    $"{Title} (Copy)",
                    Instructions,
                    Category,
                    DateTime.Now,
                    App.UserManager.CurrentUser,
                    string.Join(", ", Recipe.Ingredients));

                _recipeManager.AddRecipe(copiedRecipe);

                MessageBox.Show($"Recipe '{Title}' copied successfully!", "Copied", MessageBoxButton.OK, MessageBoxImage.Information);

                Application.Current.MainWindow.Content = new Views.RecipeListUserControl(_recipeManager);


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying recipe: {ex.Message}", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            Application.Current.MainWindow.Content = new Views.RecipeListUserControl(_recipeManager);
        }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) =>
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
}
