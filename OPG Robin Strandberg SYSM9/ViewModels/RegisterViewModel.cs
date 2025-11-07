using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using OPG_Robin_Strandberg_SYSM9.Managers;

namespace OPG_Robin_Strandberg_SYSM9.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly UserManager _userManager;

        public List<string> Countries { get; }

        public RegisterViewModel()
        {
            _userManager = App.UserManager;

            Countries = new List<string>()
            {
                "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Antigua and Barbuda",
                "Argentina", "Armenia", "Australia", "Austria", "Azerbaijan", "Bahamas",
                "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize",
                "Benin", "Bhutan", "Bolivia", "Bosnia and Herzegovina", "Botswana",
                "Brazil", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia",
                "Cameroon", "Canada", "Chile", "China", "Colombia", "Costa Rica", "Croatia",
                "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominican Republic",
                "Ecuador", "Egypt", "El Salvador", "Estonia", "Ethiopia", "Finland",
                "France", "Germany", "Ghana", "Greece", "Greenland", "Guatemala", "Honduras",
                "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland",
                "Israel", "Italy", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya",
                "Kuwait", "Latvia", "Lebanon", "Liberia", "Libya", "Lithuania", "Luxembourg",
                "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Mexico",
                "Moldova", "Monaco", "Mongolia", "Montenegro", "Morocco", "Mozambique",
                "Namibia", "Nepal", "Netherlands", "New Zealand", "Nicaragua", "Niger",
                "Nigeria", "North Korea", "Norway", "Oman", "Pakistan", "Palestine",
                "Panama", "Paraguay", "Peru", "Philippines", "Poland", "Portugal",
                "Qatar", "Romania", "Russia", "Rwanda", "Saudi Arabia", "Senegal",
                "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia",
                "South Africa", "South Korea", "Spain", "Sri Lanka", "Sudan", "Sweden",
                "Switzerland", "Syria", "Taiwan", "Tanzania", "Thailand", "Tunisia",
                "Turkey", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom",
                "United States", "Uruguay", "Uzbekistan", "Venezuela", "Vietnam", "Yemen",
                "Zambia", "Zimbabwe"
            };
        }

        public bool CreateUser(string username, string password, string country)
        {
            return _userManager.Register(username, password, country);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
