﻿using System.Windows;
using System.Windows.Controls;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.ViewModels;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    public partial class RecipeListUserControl : UserControl
    {
        private readonly RecipeManager _recipeManager;

        public RecipeListUserControl(RecipeManager recipeManager)
        {
            InitializeComponent();
            _recipeManager = recipeManager;
            DataContext = new RecipeListViewModel(_recipeManager, App.UserManager);
        }
    }
}
