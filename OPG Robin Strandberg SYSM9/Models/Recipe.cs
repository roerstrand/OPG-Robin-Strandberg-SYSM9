using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OPG_Robin_Strandberg_SYSM9.Models
{
    public class Recipe : INotifyPropertyChanged
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(Title);
            }
        }

        private string _instructions;

        public string Instructions
        {
            get { return _instructions; }
            set
            {
                _instructions = value;
                OnPropertyChanged(Instructions);
            }
        }

        private string _category;

        public string Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged(Category);
            }
        }

        private DateTime _createdAt = DateTime.Now;

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                _createdAt = value;
                OnPropertyChanged(CreatedAt.ToString());
            }
        }

        private User _createdBy;

        public User CreatedBy
        {
            get => _createdBy;
            set
            {
                _createdBy = value;
                OnPropertyChanged(CreatedBy.ToString());
            }
        }

        private List<string> _ingredients;

        public List<string> Ingredients
        {
            get => _ingredients;
            set
            {
                _ingredients.Add(value.ToString());
                OnPropertyChanged(Ingredients.ToString());
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

            _ingredients = ingredients.Split(',').ToList();
        }

        public void EditRecipe(string title, string instructions, string category, DateTime createdAt,
            User createdBy, string ingredients)
        {
            Title = title;
            Instructions = instructions;
            Category = category;
            CreatedAt = createdAt;
            CreatedBy = createdBy;

            _ingredients = ingredients.Split(',').ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    // public void CopyRecipe(string title)
    // {
    //     Console.WriteLine($"Recipe {title} was copied.");
    // }
}
