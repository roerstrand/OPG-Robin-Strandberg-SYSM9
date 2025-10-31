using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace OPG_Robin_Strandberg_SYSM9.Models
{
    public class Recipe : INotifyPropertyChanged
    {
        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _instructions;

        public string Instructions
        {
            get => _instructions;
            set
            {
                if (_instructions != value)
                {
                    _instructions = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _category;

        public string Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _createdAt = DateTime.Now;

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                if (_createdAt != value)
                {
                    _createdAt = value;
                    OnPropertyChanged();
                }
            }
        }

        private User _createdBy;

        public User CreatedBy
        {
            get => _createdBy;
            set
            {
                if (_createdBy != value)
                {
                    _createdBy = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<string> _ingredients = new List<string>();

        public List<string> Ingredients
        {
            get => _ingredients;
            set
            {
                if (value != null)
                {
                    _ingredients = value;
                    OnPropertyChanged();
                }
            }
        }

        public Recipe(string title, string instructions, string category, DateTime createdAt, User createdBy,
            string ingredients)
        {
            Title = title;
            Instructions = instructions;
            Category = category;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            Ingredients = ingredients.Split(',').Select(i => i.Trim()).ToList();
        }

        public void EditRecipe(string title, string instructions, string category, DateTime createdAt, User createdBy,
            string ingredients)
        {
            Title = title;
            Instructions = instructions;
            Category = category;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            Ingredients = ingredients.Split(',').Select(i => i.Trim()).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
