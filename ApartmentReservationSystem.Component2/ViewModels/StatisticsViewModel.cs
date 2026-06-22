using ApartmentReservationSystem.Component2.Adapters;
using ApartmentReservationSystem.Component2.Services;
using ApartmentReservationSystem.Component2.Stores;
using ApartmentReservationSystem.Component2.Strategies;

namespace ApartmentReservationSystem.Component2.ViewModels;

public class StatisticsViewModel
{
    private readonly StatisticsDataStore _dataStore = new();
    private readonly IRecordStorageAdapter _adapter = new DictionaryRecordStorageAdapter();
    private readonly InformationSystemClient _client;
    private readonly CsvExportService _csvExportService = new();

    private Guid _selectedApartmentId;
    private int _selectedMonth = DateTime.Now.Month;
    private IStatisticsMethod? _selectedMethod;
    private string _result = string.Empty;

    public StatisticsViewModel(InformationSystemClient client)
    {
        _client = client;
    }

    public Guid SelectedApartmentId
    {
        get => _selectedApartmentId;
        set => _selectedApartmentId = value;
    }

    public int SelectedMonth
    {
        get => _selectedMonth;
        set => _selectedMonth = value;
    }

    public IStatisticsMethod? SelectedMethod
    {
        get => _selectedMethod;
        set => _selectedMethod = value;
    }

    public string Result
    {
        get => _result;
        set => _result = value;
    }

    public StatisticsDataStore DataStore => _dataStore;

    public void LoadData()
    {
        var records = _client.RequestRecords(SelectedApartmentId, SelectedMonth);
        _adapter.Store(records, _dataStore);
    }

    public void Calculate()
    {
        if (_selectedMethod is null)
        {
            Result = "Izaberite statisticku metodu.";
            return;
        }

        var allRecords = _dataStore.GetAllEntries().SelectMany(pair => pair.Value).ToList();
        Result = _selectedMethod.Calculate(allRecords);
    }

    public void ExportCsv(string path)
    {
        _csvExportService.Export(Result, path);
    }
}
