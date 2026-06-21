using System.IO;
using System.Windows;
using ApartmentReservationSystem.Component1.Repositories;
using ApartmentReservationSystem.Component1.Services;
using ApartmentReservationSystem.Component1.ViewModels;
using ApartmentReservationSystem.Component1.Views;
using ApartmentReservationSystem.Component1.Wcf;

namespace ApartmentReservationSystem.Component1;

public partial class App : Application
{
    private WcfHostService? _wcfHost;

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
            var dataDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ApartmentReservationSystem",
                "Component1");

            var logPath = Path.Combine(dataDirectory, "activity.log");
            var logger = new LoggingService(logPath);

            var apartmentRepository = new ApartmentRepository();
            var recordRepository = new OccupancyRecordRepository();

            var apartmentService = new ApartmentService(apartmentRepository, logger);
            var recordService = new OccupancyRecordService(recordRepository, apartmentRepository, logger);
            var persistenceService = new PersistenceService(apartmentRepository, recordRepository, logger);
            var stateSimulationService = new StateSimulationService(logger);
            var chartService = new ChartService(recordRepository);
            var undoRedoService = new UndoRedoService();

            var viewModel = new MainViewModel(
                apartmentService,
                recordService,
                undoRedoService,
                persistenceService,
                stateSimulationService,
                chartService,
                logger,
                dataDirectory);

            var mainWindow = new MainWindow
            {
                DataContext = viewModel
            };

            MainWindow = mainWindow;
            mainWindow.Show();

            logger.Log("Aplikacija informacionog sistema je pokrenuta.");

            StartWcfHost(apartmentService, recordService, logger);
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

    private void StartWcfHost(
        ApartmentService apartmentService,
        OccupancyRecordService recordService,
        LoggingService logger)
    {
        _ = Task.Run(() =>
        {
            try
            {
                var wcfService = new InformationSystemService(apartmentService, recordService);
                _wcfHost = new WcfHostService();
                _wcfHost.Start(wcfService);
                logger.Log($"WCF servis pokrenut na {WcfHostService.ServiceUrl}");
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        $"WCF servis nije pokrenut: {ex.Message}\n\nAplikacija ce raditi, ali Komponenta 2 nece moci da preuzme podatke dok se ovo ne resi.",
                        "WCF upozorenje",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                });
                logger.Log($"GRESKA WCF: {ex.Message}");
            }
        });
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        _wcfHost?.Stop();
    }
}
