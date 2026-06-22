using System.Windows;
using ApartmentReservationSystem.Component2.Services;
using ApartmentReservationSystem.Component2.ViewModels;

namespace ApartmentReservationSystem.Component2;

public partial class App : Application
{
    private InformationSystemConnection? _connection;

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        DispatcherUnhandledException += (_, args) =>
        {
            MessageBox.Show(
                args.Exception.Message,
                "Greska u aplikaciji",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            args.Handled = true;
        };

        try
        {
            _connection = new InformationSystemConnection();
            var connectionError = _connection.Connect();

            if (connectionError is not null)
            {
                MessageBox.Show(
                    $"Nije moguce povezati se sa Komponentom 1.\n\n{connectionError}\n\n" +
                    "Pokrenite informacioni sistem (Komponenta 1) i ponovo pokrenite ovu aplikaciju.",
                    "WCF upozorenje",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            var client = new InformationSystemClient(_connection);
            var viewModel = new StatisticsViewModel(client);

            var mainWindow = new MainWindow
            {
                DataContext = viewModel
            };

            MainWindow = mainWindow;
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Greska pri pokretanju",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown(-1);
        }
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        _connection?.Dispose();
    }
}
