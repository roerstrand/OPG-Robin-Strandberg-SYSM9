using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPG_Robin_Strandberg_SYSM9.Models
{
    internal class Recipe
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _instructions;

        public string Instructions
        {
            get { return _instructions; }
            set
            {
                _instructions = value;
                OnPropertyChanged();
            }
        }

        private string _category;

        public string Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        private DateTime _createdAt = DateTime.Now;

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
            OnPropertyChanged();
        }

        private User _createdBy;

        public User CreatedBy
        {
            get => _createdBy;
            set => _createdBy = value;
            OnPropertyChanged();
        }

        private List<string> _ingredients;

        public List<string> Ingredients
        {
            get => _ingredients;
            set => _ingredients += value;
            OnPropertyChanged();
        }

        public Recipe(string title, string instructions, string category, DateTime createdAt, User createdBy,
            List<string. string, string> ingredients)
        {
            Title = title;
            Instructions = instructions;
            Category = category;
            CreatedAt = createdAt;
            CreatedBy = createdBy;

            _ingredients = new List<string>(ingredients);
        }

        public void EditRecipe(string title, string instructions, string category, DateTime createdAt, User createdBy,
            List<string. string, string> ingredients)
        {
            Title = title;
            Instructions = instructions;
            Category = category;
            CreatedAt = createdAt;
            CreatedBy = createdBy;

            _ingredients = new List<string>(ingredients);
        }
            
    }
    
    public Recipe CopyRecipe(string title)
    {
        Console.WriteLine($"{Recipe.Title} was copied.");
        return Recipe;
    }
}