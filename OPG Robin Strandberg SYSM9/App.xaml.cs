using System.Configuration;
using System.Data;
using System.Windows;
using OPG_Robin_Strandberg_SYSM9.Managers;

namespace OPG_Robin_Strandberg_SYSM9
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        // Skapar statiska egenskaper som ger åtkomst till respektive managerinstans i app.xaml
        public static UserManager UserManager => (UserManager)Current.Resources["UserManager"];

    }
}
