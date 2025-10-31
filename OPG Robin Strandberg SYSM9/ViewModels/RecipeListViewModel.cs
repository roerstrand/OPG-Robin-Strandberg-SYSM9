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
                    MessageBox.Show("Ingen användare är inloggad. Återgår till startsidan.", "Session",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Recipes = new ObservableCollection<Recipe>(
                    _recipeManager.GetByUser(_userManager.CurrentUser)
                );
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte läsa in recept.", ex);
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
                ShowError("Fel vid filtrering.", ex);
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
                ShowError("Kunde inte öppna Add Recipe-vyn.", ex);
            }
        }


        private void RemoveRecipe()
        {
            try
            {
                if (SelectedRecipe == null)
                {
                    MessageBox.Show("Markera ett recept att ta bort.", "Varning", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show($"Vill du ta bort \"{SelectedRecipe.Title}\"?", "Bekräfta", MessageBoxButton.YesNo,
                        MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _recipeManager.RemoveRecipe(SelectedRecipe);
                    LoadRecipes();
                }
            }
            catch (Exception ex)
            {
                ShowError("Fel vid borttagning av recept.", ex);
            }
        }

        private void ShowDetails()
        {
            try
            {
                if (SelectedRecipe == null)
                {
                    MessageBox.Show("Markera ett recept att visa.", "Varning", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                var detailsControl = new Views.RecipeDetailUserControl(SelectedRecipe, _recipeManager);
                Application.Current.MainWindow.Content = detailsControl;

            }
            catch (Exception ex)
            {
                ShowError("Kunde inte öppna detaljvyn.", ex);
            }
        }

        private void ShowInfo()
        {
            MessageBox.Show(
                "CookMaster låter dig skapa, visa och hantera dina recept.\n\nAnvänd filtrering för att söka efter titel eller kategori.",
                "Om CookMaster", MessageBoxButton.OK, MessageBoxImage.Information);
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
                ShowError("Fel vid utloggning.", ex);
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
            MessageBox.Show($"{message}\n\n{ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
