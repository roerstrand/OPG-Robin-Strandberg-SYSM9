using System.Windows;
using OPG_Robin_Strandberg_SYSM9.Managers;

namespace OPG_Robin_Strandberg_SYSM9
{
    public partial class App : Application
    {
        public static UserManager UserManager { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            UserManager ??= new UserManager();

            MainWindow = new Views.MainWindow();
            MainWindow.Show();
        }
    }
}
