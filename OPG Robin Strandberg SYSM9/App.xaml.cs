using System.Windows;
using System.Windows.Threading;
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

            DispatcherUnhandledException +=
                App_DispatcherUnhandledException; // Global felhanterare av fel i UI-tråden
            AppDomain.CurrentDomain.UnhandledException +=
                CurrentDomain_UnhandledException; // felhanterade för bakgrundstrådar i applikationen
        }

        public void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
            MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        public void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject);
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show("An unexpected error occured with the application. The application will now close." +
                                "Please open the application again.");
            });
        }
    }
}
