using System;
using System.Collections.ObjectModel;
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
    public class RecipeListViewModel : INotifyPropertyChanged
    {
        private readonly RecipeManager _recipeManager;
        private readonly UserManager _userManager;

        public bool IsAdminVisible => _userManager.ActiveAdmins.Contains(_userManager.CurrentUser);

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
        public ICommand RecipeDetailsCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand InfoCommand { get; }
        public ICommand SignOutCommand { get; }
        public ICommand ShowUserDetailsCommand { get; }

        public ICommand ShowAllRecipesCommand { get; }

        public RecipeListViewModel(RecipeManager recipeManager, UserManager userManager)
        {
            _recipeManager = recipeManager;
            _userManager = userManager;

            AddRecipeCommand = new RelayCommand(_ => AddRecipe());
            RemoveRecipeCommand = new RelayCommand(_ => RemoveRecipe());
            RecipeDetailsCommand = new RelayCommand(_ => ShowRecipeDetails());
            FilterCommand = new RelayCommand(_ => FilterRecipes());
            InfoCommand = new RelayCommand(_ => ShowInfo());
            SignOutCommand = new RelayCommand(_ => SignOut());
            ShowUserDetailsCommand = new RelayCommand(_ => ShowUserDetails());

            ShowAllRecipesCommand = new RelayCommand(_ => ShowAllRecipes(),
                _ => _userManager.ActiveAdmins.Contains(_userManager.CurrentUser));


            LoadRecipes();
        }

        private void LoadRecipes()
        {
            try
            {
                if (_userManager.CurrentUser == null)
                {
                    MessageBox.Show("No user is logged in. Returning to start screen.",
                        "Session", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_userManager.ActiveAdmins.Contains(_userManager.CurrentUser))
                    Recipes = _recipeManager.GetAllRecipesForAdmin(_userManager.Users);
                else
                    Recipes = new ObservableCollection<Recipe>(_recipeManager.GetByUser(_userManager.CurrentUser));
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
            var addRecipe = new AddRecipeWindow(_recipeManager);
            addRecipe.Show();
            CloseCurrentWindow();
        }

        private void RemoveRecipe()
        {
            if (SelectedRecipe == null)
            {
                MessageBox.Show("Select a recipe to remove.",
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Do you want to delete \"{SelectedRecipe.Title}\"?",
                    "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _recipeManager.RemoveRecipe(SelectedRecipe);
                LoadRecipes();
            }
        }

        private void ShowRecipeDetails()
        {
            if (SelectedRecipe == null)
            {
                MessageBox.Show("Select a recipe to view.",
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var detailsWindow = new RecipeDetailsWindow(SelectedRecipe, _recipeManager);
            detailsWindow.Show();
            CloseCurrentWindow();
        }


        private void ShowUserDetails()
        {
            try
            {
                var userDetails = new UserDetailsWindow(_userManager);
                userDetails.Show();
                CloseCurrentWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open user details.\n\n{ex.Message}", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        private void ShowInfo()
        {
            MessageBox.Show(
                "Welcome to CookMaster\n\n" +
                "CookMaster is a modern recipe management platform designed to make it easy to collect, organize, and share cooking inspiration.\n\n" +
                "USER GUIDE\n" +
                "• Regular users can create, edit, and manage their own recipes.\n" +
                "• Recipes can be filtered by title or category for quick access.\n\n" +
                "ADMIN ACCESS\n" +
                "• Admin users can view and manage all submitted recipes.\n" +
                "• Ensures quality and consistency in the shared recipe database.\n\n" +
                "ABOUT COOKMASTER\n" +
                "CookMaster is developed with a focus on simplicity, creativity, and community. Our goal is to bring structure and joy to everyday cooking while offering a clean and user-friendly experience.\n\n" +
                "Thank you for using CookMaster.",
                "About CookMaster",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        private void SignOut()
        {
            _userManager.Logout();
            var mainWindow = new MainWindow();
            mainWindow.Show();
            CloseCurrentWindow();
        }

        private void CloseCurrentWindow()
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.DataContext == this)
                {
                    w.Close();
                    break;
                }
            }
        }

        private void ShowAllRecipes()
        {
            try
            {
                if (_userManager.ActiveAdmins.Contains(_userManager.CurrentUser))
                {
                    Recipes = _recipeManager.ViewAllRecipes(
                        _userManager.CurrentUser,
                        _userManager.ActiveAdmins.Select(a => (User)a).ToList()
                    );
                }
                else
                {
                    MessageBox.Show("Only admins can view all recipes.",
                        "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                ShowError("Could not load all recipes.", ex);
            }
        }


        private void ShowError(string message, Exception ex)
        {
            MessageBox.Show($"{message}\n\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
