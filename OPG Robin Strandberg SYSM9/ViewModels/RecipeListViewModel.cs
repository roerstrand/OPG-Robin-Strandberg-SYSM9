using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using OPG_Robin_Strandberg_SYSM9;
using OPG_Robin_Strandberg_SYSM9.Models;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Commands;
using OPG_Robin_Strandberg_SYSM9.Views;

namespace OPG_Robin_Strandberg_SYSM9.ViewModels
{
    public class RecipeListViewModel : INotifyPropertyChanged
    {
        private readonly RecipeManager _recipeManager;
        private readonly UserManager _userManager;

        private ObservableCollection<Recipe> _recipes;

        public ObservableCollection<Recipe> Recipes
        {
            get => _recipes;
            set
            {
                _recipes = value;
                OnPropertyChanged();
            }
        }

        private Recipe _selectedRecipe;

        public Recipe SelectedRecipe
        {
            get => _selectedRecipe;
            set
            {
                _selectedRecipe = value;
                OnPropertyChanged();
            }
        }

        private string _filterText;

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged();
            }
        }

        public string CurrentUserName => _userManager?.CurrentUser?.UserName ?? "Unknown";

        public ICommand AddRecipeCommand { get; }
        public ICommand RemoveRecipeCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand InfoCommand { get; }
        public ICommand SignOutCommand { get; }

        public RecipeListViewModel(RecipeManager recipeManager, UserManager userManager)
        {
            _recipeManager = recipeManager;
            _userManager = userManager;

            AddRecipeCommand = new RelayCommand(_ => AddRecipe());
            RemoveRecipeCommand = new RelayCommand(_ => RemoveRecipe());
            DetailsCommand = new RelayCommand(_ => ShowDetails());
            FilterCommand = new RelayCommand(_ => FilterRecipes());
            InfoCommand = new RelayCommand(_ => ShowInfo());
            SignOutCommand = new RelayCommand(_ => SignOut());

            LoadRecipes();
        }

        private void LoadRecipes()
        {
            try
            {
                if (_userManager.CurrentUser == null)
                {
                    MessageBox.Show("No user is logged in. Returning to start screen.", "Session",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Recipes = new ObservableCollection<Recipe>(
                    _recipeManager.GetByUser(_userManager.CurrentUser)
                );
            }
            catch (Exception ex)
            {
                ShowError("Could not load recipes.", ex);
            }
        }

        private void FilterRecipes()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FilterText))
                {
                    LoadRecipes();
                    return;
                }

                Recipes = _recipeManager.Filter(FilterText);
            }
            catch (Exception ex)
            {
                ShowError("Error while filtering.", ex);
            }
        }

        private void AddRecipe()
        {
            try
            {
                // byt ut UserControl i fönstret istället för att öppna nytt
                Application.Current.MainWindow.Content = new Views.AddRecipeUserControl(_recipeManager);
            }
            catch (Exception ex)
            {
                ShowError("Could not open Add Recipe view.", ex);
            }
        }

        private void RemoveRecipe()
        {
            try
            {
                if (SelectedRecipe == null)
                {
                    MessageBox.Show("Select a recipe to remove.", "Warning", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show($"Do you want to delete \"{SelectedRecipe.Title}\"?", "Confirm", MessageBoxButton.YesNo,
                        MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _recipeManager.RemoveRecipe(SelectedRecipe);
                    LoadRecipes();
                }
            }
            catch (Exception ex)
            {
                ShowError("Error while deleting recipe.", ex);
            }
        }

        private void ShowDetails()
        {
            try
            {
                if (SelectedRecipe == null)
                {
                    MessageBox.Show("Select a recipe to view.", "Warning", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                var detailsControl = new Views.RecipeDetailUserControl(SelectedRecipe, _recipeManager);
                Application.Current.MainWindow.Content = detailsControl;
            }
            catch (Exception ex)
            {
                ShowError("Could not open detail view.", ex);
            }
        }

        private void ShowInfo()
        {
            MessageBox.Show(
                "CookMaster lets you create, view, and manage your recipes.\n\nUse filtering to search by title or category.",
                "About CookMaster", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SignOut()
        {
            try
            {
                _userManager.Logout();
                var mainWindow = new Views.MainWindow();
                mainWindow.Show();
                CloseCurrentWindow();
            }
            catch (Exception ex)
            {
                ShowError("Error during logout.", ex);
            }
        }

        private void CloseCurrentWindow()
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Views.RecipeListUserControl)
                {
                    w.Close();
                    break;
                }
            }
        }

        private void ShowError(string message, Exception ex)
        {
            Console.WriteLine($"[RecipeListVM] {ex.Message}");
            MessageBox.Show($"{message}\n\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
