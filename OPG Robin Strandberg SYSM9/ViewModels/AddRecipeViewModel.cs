﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using OPG_Robin_Strandberg_SYSM9.Commands;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Models;

namespace OPG_Robin_Strandberg_SYSM9.ViewModels
{
    public class AddRecipeViewModel : INotifyPropertyChanged
    {
        private readonly RecipeManager _recipeManager;

        private string _title;
        private string _ingredients;
        private string _instructions;
        private string _category;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string Ingredients
        {
            get => _ingredients;
            set { _ingredients = value; OnPropertyChanged(); }
        }

        public string Instructions
        {
            get => _instructions;
            set { _instructions = value; OnPropertyChanged(); }
        }

        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddRecipeViewModel(RecipeManager recipeManager)
        {
            _recipeManager = recipeManager;

            SaveCommand = new RelayCommand(_ => SaveRecipe());
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
                    MessageBox.Show("Alla fält måste fyllas i.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var recipe = new Recipe(
                    Title,
                    Instructions,
                    Category,
                    DateTime.Now,
                    App.UserManager.CurrentUser,
                    Ingredients
                );

                _recipeManager.AddRecipe(recipe);

                MessageBox.Show("Recept tillagt!", "Klart", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ett fel uppstod: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            Application.Current.Windows.OfType<Window>()
                .FirstOrDefault(w => w.DataContext == this)
                ?.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
