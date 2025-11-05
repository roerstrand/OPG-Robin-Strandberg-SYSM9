using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using OPG_Robin_Strandberg_SYSM9.Commands;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Models;
using OPG_Robin_Strandberg_SYSM9.Views;

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
            set
            {
                _recipe = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(Ingredients));
                OnPropertyChanged(nameof(Instructions));
                OnPropertyChanged(nameof(Category));
            }
        }


        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }

        public bool IsReadOnly => !IsEditing;

        public string Title
        {
            get => Recipe.Title;
            set
            {
                Recipe.Title = value;
                OnPropertyChanged();
            }
        }

        public string Ingredients
        {
            get => string.Join(", ", Recipe.Ingredients);
            set
            {
                Recipe.Ingredients = value.Split(',').Select(i => i.Trim()).ToList();
                OnPropertyChanged();
            }
        }

        public string Instructions
        {
            get => Recipe.Instructions;
            set
            {
                Recipe.Instructions = value;
                OnPropertyChanged();
            }
        }

        public string Category
        {
            get => Recipe.Category;
            set
            {
                Recipe.Category = value;
                OnPropertyChanged();
            }
        }

        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CopyCommand { get; }
        public ICommand CancelCommand { get; }

        public RecipeDetailViewModel(Recipe recipe, RecipeManager recipeManager)
        {
            Recipe = recipe;
            _recipeManager = recipeManager;

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
                    string.IsNullOrWhiteSpace(Ingredients))
                {
                    MessageBox.Show("All fields must be filled in.", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                _recipeManager.UpdateRecipe(
                    Recipe,
                    Title,
                    Instructions,
                    Category,
                    Ingredients
                );

                MessageBox.Show("Recipe updated successfully!", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);

                var listWindow = new RecipeListWindow(_recipeManager);
                listWindow.Show();

                foreach (Window w in Application.Current.Windows)
                {
                    if (w.DataContext == this)
                    {
                        w.Close();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the recipe:\n" + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CopyRecipe()
        {
            try
            {
                var copy = new Recipe(
                    $"{Title} (Copy)",
                    Instructions,
                    Category,
                    DateTime.Now,
                    App.UserManager.CurrentUser,
                    string.Join(", ", Recipe.Ingredients)
                );

                _recipeManager.AddRecipe(copy);
                MessageBox.Show("Recipe copied successfully!", "Copied", MessageBoxButton.OK,
                    MessageBoxImage.Information);

                var listWindow = new RecipeListWindow(_recipeManager);
                listWindow.Show();

                foreach (Window w in Application.Current.Windows)
                {
                    if (w.DataContext == this)
                    {
                        w.Close();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void Cancel()
        {
            try
            {
                var listWindow = new RecipeListWindow(_recipeManager);
                listWindow.Show();

                foreach (Window w in Application.Current.Windows)
                {
                    if (w.DataContext == this)
                    {
                        w.Close();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
