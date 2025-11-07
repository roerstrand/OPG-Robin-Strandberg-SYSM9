using System.Windows;
using System.Windows.Threading;
using OPG_Robin_Strandberg_SYSM9.Managers;
using OPG_Robin_Strandberg_SYSM9.Views;

namespace OPG_Robin_Strandberg_SYSM9
{
    public partial class App : Application
    {
        public static UserManager UserManager { get; private set; }

        // Samtliga resurser laddas i application-metoden OnStartUp efter färdkompilerad app-klass ist
        // globala resurser satta i app.xaml. Men körs innan något fönster har öppnats.
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Application klass base metod OnSartUp laddar globalt med absolut filväg
            var theme = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/OPG%20Robin%20Strandberg%20SYSM9;component/Themes/GlobalStyles.xaml", UriKind.Absolute)
            };

            Application.Current.Resources.MergedDictionaries.Add(theme);

            UserManager ??= new UserManager();

            MainWindow = new Views.MainWindow();
            MainWindow.Show();

            DispatcherUnhandledException +=
                App_DispatcherUnhandledException; // Global felhanterare av fel i UI-tråden
            AppDomain.CurrentDomain.UnhandledException +=
                CurrentDomain_UnhandledException; // felhanterade för bakgrundstrådar i applikationen

            ShutdownMode = ShutdownMode.OnExplicitShutdown; // Förhindrar att app avslutas om alla fönster stängs så inte
            // tilladga recept, användare eller ändringar i användardata går förlorad.
        }

        // Om användaren råkar stänga alla fönster i appen, skicka förfrågan om de vill öppna appen igen (appens fortsätta köra ändå enligt ShutDownMode)
        protected override void OnExit(ExitEventArgs e)
        {
            if (Current.MainWindow == null)
            {
                var result = MessageBox.Show(
                    "All windows are closed. Do you want to reopen the app?",
                    "App still running",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    var main = new MainWindow();
                    main.Show();
                }
            }

            base.OnExit(e);
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
