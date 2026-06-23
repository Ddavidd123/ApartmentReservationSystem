using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using ApartmentReservationSystem.Component2.Adapters;
using ApartmentReservationSystem.Component2.Commands;
using ApartmentReservationSystem.Component2.Helpers;
using ApartmentReservationSystem.Component2.Models;
using ApartmentReservationSystem.Component2.Services;
using ApartmentReservationSystem.Component2.Stores;
using ApartmentReservationSystem.Component2.Strategies;
using ApartmentReservationSystem.Shared.Models;
using Microsoft.Win32;

namespace ApartmentReservationSystem.Component2.ViewModels;

public class StatisticsViewModel : ViewModelBase
{
    private readonly StatisticsDataStore _dataStore = new();
    private readonly IRecordStorageAdapter _adapter = new DictionaryRecordStorageAdapter();
    private readonly InformationSystemClient _client;
    private readonly CsvExportService _csvExportService = new();

    private Apartment? _selectedApartment;
    private MonthOption? _selectedMonthOption;
    private IStatisticsMethod? _selectedMethod;
    private string _result = string.Empty;
    private string _statusMessage = string.Empty;

    public StatisticsViewModel(InformationSystemClient client)
    {
        _client = client;

        AvailableMethods =
        [
            new AverageDailyRateStrategy(),
            new MaxRevenueStrategy(),
            new LowRatingReservationsStrategy()
        ];

        Months = CreateMonthOptions();
        _selectedMonthOption = Months.FirstOrDefault(month => month.Number == DateTime.Now.Month);

        LoadDataCommand = new RelayCommand(LoadData, CanLoadData);
        CalculateCommand = new RelayCommand(Calculate, () => SelectedMethod is not null);
        ExportCsvCommand = new RelayCommand(ExportCsv, () => _dataStore.GetAllEntries().Count > 0);

        RefreshApartments();
    }

    public ObservableCollection<Apartment> Apartments { get; } = [];

    public ObservableCollection<string> DisplayRecords { get; } = [];

    public IReadOnlyList<IStatisticsMethod> AvailableMethods { get; }

    public IReadOnlyList<MonthOption> Months { get; }

    public RelayCommand LoadDataCommand { get; }

    public RelayCommand CalculateCommand { get; }

    public RelayCommand ExportCsvCommand { get; }

    public bool IsConnected => _client.IsConnected;

    public Apartment? SelectedApartment
    {
        get => _selectedApartment;
        set
        {
            if (SetProperty(ref _selectedApartment, value))
            {
                LoadDataCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public MonthOption? SelectedMonthOption
    {
        get => _selectedMonthOption;
        set
        {
            if (SetProperty(ref _selectedMonthOption, value))
            {
                LoadDataCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public IStatisticsMethod? SelectedMethod
    {
        get => _selectedMethod;
        set
        {
            if (SetProperty(ref _selectedMethod, value))
            {
                CalculateCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public string Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public StatisticsDataStore DataStore => _dataStore;

    public void RefreshApartments(bool updateStatusMessage = true)
    {
        var previouslySelectedId = SelectedApartment?.Id;
        var previousCount = Apartments.Count;

        Apartments.Clear();

        if (!IsConnected)
        {
            if (updateStatusMessage)
            {
                StatusMessage = "Nema konekcije sa Komponentom 1.";
            }

            SelectedApartment = null;
            LoadDataCommand.RaiseCanExecuteChanged();
            return;
        }

        try
        {
            foreach (var apartment in _client.GetApartments())
            {
                Apartments.Add(apartment);
            }

            SelectedApartment = previouslySelectedId is Guid id
                ? Apartments.FirstOrDefault(apartment => apartment.Id == id)
                : Apartments.FirstOrDefault();

            if (SelectedApartment is null && Apartments.Count > 0)
            {
                SelectedApartment = Apartments[0];
            }

            if (updateStatusMessage || Apartments.Count != previousCount)
            {
                StatusMessage = Apartments.Count == 0
                    ? "Komponenta 1 nema unetih apartmana."
                    : $"Ucitano {Apartments.Count} apartmana.";
            }

            LoadDataCommand.RaiseCanExecuteChanged();
        }
        catch (Exception ex)
        {
            if (updateStatusMessage)
            {
                StatusMessage = ex.Message;
            }
        }
    }

    private bool CanLoadData()
    {
        return IsConnected &&
               SelectedApartment is not null &&
               SelectedMonthOption is not null;
    }

    private void LoadData()
    {
        if (SelectedApartment is null || SelectedMonthOption is null)
        {
            StatusMessage = "Izaberite apartman i mesec.";
            return;
        }

        try
        {
            RefreshApartments(updateStatusMessage: false);

            if (SelectedApartment is null)
            {
                StatusMessage = "Nema dostupnih apartmana u Komponenti 1.";
                return;
            }

            var records = _client.RequestRecords(SelectedApartment.Id, SelectedMonthOption.Number);
            _adapter.Store(records, _dataStore);
            RefreshDisplayRecords();

            StatusMessage = records.Count == 0
                ? "Nema zapisa za izabrani apartman i mesec."
                : $"Ucitano {records.Count} zapisa.";

            Result = string.Empty;
            ExportCsvCommand.RaiseCanExecuteChanged();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private void RefreshDisplayRecords()
    {
        DisplayRecords.Clear();

        foreach (var entry in _dataStore.GetAllEntries().OrderBy(pair => pair.Key))
        {
            foreach (var record in entry.Value)
            {
                DisplayRecords.Add(RecordDisplayFormatter.Format(record));
            }
        }
    }

    private void Calculate()
    {
        if (SelectedMethod is null)
        {
            Result = "Izaberite statisticku metodu.";
            return;
        }

        var allRecords = _dataStore.GetAllEntries().SelectMany(pair => pair.Value).ToList();

        if (allRecords.Count == 0)
        {
            Result = "Nema zapisa za izracunavanje.";
            return;
        }

        Result = SelectedMethod.Calculate(allRecords);
    }

    private void ExportCsv()
    {
        var records = _dataStore.GetAllEntries()
            .SelectMany(pair => pair.Value)
            .ToList();

        if (records.Count == 0)
        {
            StatusMessage = "Nema zapisa za izvoz u CSV.";
            return;
        }

        var dialog = new SaveFileDialog
        {
            Filter = "CSV fajl (*.csv)|*.csv",
            FileName = "occupancy-records.csv"
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            _csvExportService.Export(
                records,
                SelectedMethod?.Name,
                string.IsNullOrWhiteSpace(Result) ? null : Result,
                dialog.FileName);

            StatusMessage = $"CSV sacuvan: {dialog.FileName}";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
            MessageBox.Show(ex.Message, "Greska pri izvozu", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private static List<MonthOption> CreateMonthOptions()
    {
        return CultureInfo
            .GetCultureInfo("sr-Latn-RS")
            .DateTimeFormat
            .MonthNames
            .Take(12)
            .Select((name, index) => new MonthOption
            {
                Number = index + 1,
                Name = char.ToUpper(name[0]) + name[1..]
            })
            .ToList();
    }
}
