using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Models;
using OPG_Robin_Strandberg_SYSM9.Views;
using OPG_Robin_Strandberg_SYSM9.Commands;
using OPG_Robin_Strandberg_SYSM9.ViewModels;
using System;
using System.Collections.Generic;

namespace OPG_Robin_Strandberg_SYSM9
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly UserManager _userManager;
        private RecipeManager _recipeManager;

        private string _userNameInput;
        private string _passwordInput;
        private User _currentUser;
        private object _currentView;

        private RecipeListViewModel _recipeListViewModel;

        public RecipeListViewModel RecipeListViewModel
        {
            get => _recipeListViewModel;
            set
            {
                _recipeListViewModel = value;
                OnPropertyChanged();
            }
        }

        public string UserNameInput
        {
            get => _userNameInput;
            set
            {
                _userNameInput = value;
                OnPropertyChanged();
            }
        }

        public string PasswordInput
        {
            get => _passwordInput;
            set
            {
                _passwordInput = value;
                OnPropertyChanged();
            }
        }

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public List<User> Users { get; set; } = new List<User>();

        public bool IsAuthenticated => _userManager.IsAuthenticated;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowRegisterCommand { get; }
        public ICommand ShowForgotPasswordCommand { get; }
        public ICommand ShowAddRecipeCommand { get; }
        public ICommand ShowViewRecipeDetailsCommand { get; }
        public ICommand ShowViewRecipeListCommand { get; }
        public ICommand ShowViewUserDetailsCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainWindowViewModel()
        {
            _userManager = App.UserManager;
            _recipeManager = new RecipeManager();

            // Login
            LoginCommand = new RelayCommand(_ => Login_Button());
            ShowRegisterCommand = new RelayCommand(_ =>
            {
                var register = new RegisterWindow
                {
                    Owner = Application.Current.MainWindow
                };
                register.ShowDialog();
            });

            ShowForgotPasswordCommand = new RelayCommand(_ =>
            {
                var forgot = new ForgotPasswordWindow
                {
                    Owner = Application.Current.MainWindow
                };
                forgot.ShowDialog();
            });

            ShowAddRecipeCommand = new RelayCommand(_ =>
            {
                var addRecipeWindow = new AddRecipeWindow
                {
                    DataContext = new AddRecipeViewModel(_recipeManager),
                    Owner = Application.Current.MainWindow
                };
                addRecipeWindow.Show();
            });

            ShowViewRecipeListCommand = new RelayCommand(_ =>
            {
                RecipeListViewModel = new RecipeListViewModel(_recipeManager, _userManager);
                var recipeListWindow = new RecipeListWindow
                {
                    DataContext = RecipeListViewModel,
                    Owner = Application.Current.MainWindow
                };
                recipeListWindow.Show();
            });

            ShowViewRecipeDetailsCommand = new RelayCommand(_ =>
            {
                if (_recipeManager.CurrentRecipe == null)
                {
                    MessageBox.Show("Choose a recipe first.", "Information", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                var recipeDetailWindow = new RecipeDetailsWindow
                {
                    DataContext = new RecipeDetailViewModel(_recipeManager.CurrentRecipe, _recipeManager),
                    Owner = Application.Current.MainWindow
                };
                recipeDetailWindow.Show();
            });

            LogoutCommand = new RelayCommand(_ => Logout_Button());
        }

        public void Login_Button()
        {
            try
            {
                if (_userManager.Login(UserNameInput, PasswordInput))
                {
                    var recipeList = new RecipeListWindow(_userManager.GetRecipeManagerForCurrentUser());
                    recipeList.Show();

                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is MainWindow)
                        {
                            window.Close();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Logout_Button()
        {
            _userManager.Logout();
            UserNameInput = string.Empty;
            PasswordInput = string.Empty;
            OnPropertyChanged(nameof(IsAuthenticated));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
