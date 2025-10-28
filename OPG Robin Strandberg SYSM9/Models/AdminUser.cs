using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OPG_Robin_Strandberg_SYSM9.Models
{
    public class AdminUser : User, INotifyPropertyChanged
    {
        public AdminUser(string username, string password, string country) : base(username, password, country)
        {
        }
        public void RemoveAnyRecipe()
        {

        }

        public void ViewAllRecipes()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
