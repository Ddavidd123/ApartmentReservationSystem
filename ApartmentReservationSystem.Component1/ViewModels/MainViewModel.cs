using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using ApartmentReservationSystem.Component1.Commands;
using ApartmentReservationSystem.Component1.Helpers;
using ApartmentReservationSystem.Component1.Services;
using ApartmentReservationSystem.Shared.Enums;
using ApartmentReservationSystem.Shared.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace ApartmentReservationSystem.Component1.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly ApartmentService _apartmentService;
    private readonly OccupancyRecordService _recordService;
    private readonly UndoRedoService _undoRedoService;
    private readonly PersistenceService _persistenceService;
    private readonly StateSimulationService _stateSimulationService;
    private readonly ChartService _chartService;
    private readonly LoggingService _logger;

    private readonly string _jsonPath;
    private readonly string _xmlPath;

    private Apartment? _selectedApartment;
    private ApartmentOccupancyRecord? _selectedRecord;
    private string _searchCriteria = string.Empty;
    private string _statusMessage = string.Empty;
    private ISeries[] _chartSeries = [];

    private string _apartmentAddress = string.Empty;
    private int _apartmentFloor;
    private ApartmentType _apartmentType = ApartmentType.SingleBed;
    private int _apartmentSize = 30;
    private int _apartmentCapacity = 1;

    private Guid _recordApartmentId;
    private DateTime _recordCheckIn = DateTime.Today;
    private DateTime _recordCheckOut = DateTime.Today.AddDays(1);
    private double _recordDailyRate = 1000;
    private double _recordGuestRating = 3;
    private OccupancyState _recordState = OccupancyState.Available;

    public MainViewModel(
        ApartmentService apartmentService,
        OccupancyRecordService recordService,
        UndoRedoService undoRedoService,
        PersistenceService persistenceService,
        StateSimulationService stateSimulationService,
        ChartService chartService,
        LoggingService logger,
        string dataDirectory)
    {
        _apartmentService = apartmentService;
        _recordService = recordService;
        _undoRedoService = undoRedoService;
        _persistenceService = persistenceService;
        _stateSimulationService = stateSimulationService;
        _chartService = chartService;
        _logger = logger;

        Directory.CreateDirectory(dataDirectory);
        _jsonPath = Path.Combine(dataDirectory, "data.json");
        _xmlPath = Path.Combine(dataDirectory, "data.xml");

        Apartments.CollectionChanged += (_, _) => RefreshChart();
        Records.CollectionChanged += (_, _) => RefreshChart();
        _chartService.ChartDataChanged += RefreshChart;

        AddApartmentCommand = new RelayCommand(AddApartment, () => !string.IsNullOrWhiteSpace(ApartmentAddress));
        UpdateApartmentCommand = new RelayCommand(UpdateApartment, () => SelectedApartment is not null);
        DeleteApartmentCommand = new RelayCommand(DeleteApartment, () => SelectedApartment is not null);

        AddRecordCommand = new RelayCommand(AddRecord, () => RecordApartmentId != Guid.Empty);
        UpdateRecordCommand = new RelayCommand(UpdateRecord, () => SelectedRecord is not null);
        DeleteRecordCommand = new RelayCommand(DeleteRecord, () => SelectedRecord is not null);

        UndoCommand = new RelayCommand(Undo, () => _undoRedoService.CanUndo);
        RedoCommand = new RelayCommand(Redo, () => _undoRedoService.CanRedo);
        SearchCommand = new RelayCommand(Search);
        SaveJsonCommand = new RelayCommand(() => Save(false));
        SaveXmlCommand = new RelayCommand(() => Save(true));
        LoadJsonCommand = new RelayCommand(() => Load(false));
        LoadXmlCommand = new RelayCommand(() => Load(true));
        SimulateStateCommand = new RelayCommand(SimulateState, () => SelectedRecord is not null);
        MoveNextStateCommand = new RelayCommand(MoveNextState, () => SelectedRecord is not null);
        ResetSearchCommand = new RelayCommand(ResetSearch);

        Load(false);
        RefreshChart();
        StatusMessage = "Sistem spreman. Podaci se cuvaju u prikazanoj putanji ispod.";
    }

    public string DataFolderPath => Path.GetDirectoryName(_jsonPath) ?? string.Empty;
    public string XmlFilePath => _xmlPath;
    public string JsonFilePath => _jsonPath;

    public ObservableCollection<Apartment> Apartments { get; } = [];
    public ObservableCollection<ApartmentOccupancyRecord> Records { get; } = [];

    public Array ApartmentTypes => Enum.GetValues(typeof(ApartmentType));
    public Array OccupancyStates => Enum.GetValues(typeof(OccupancyState));

    public Apartment? SelectedApartment
    {
        get => _selectedApartment;
        set
        {
            if (!SetProperty(ref _selectedApartment, value) || value is null)
            {
                return;
            }

            ApartmentAddress = value.Address;
            ApartmentFloor = value.Floor;
            ApartmentType = value.Type;
            ApartmentSize = value.Size;
            ApartmentCapacity = value.Capacity;
        }
    }

    public ApartmentOccupancyRecord? SelectedRecord
    {
        get => _selectedRecord;
        set
        {
            if (!SetProperty(ref _selectedRecord, value) || value is null)
            {
                return;
            }

            RecordApartmentId = value.ApartmentId;
            RecordCheckIn = value.CheckInDate;
            RecordCheckOut = value.CheckoutDate;
            RecordDailyRate = value.DailyRate;
            RecordGuestRating = value.GuestRating;
            RecordState = value.State;
        }
    }

    public string SearchCriteria
    {
        get => _searchCriteria;
        set => SetProperty(ref _searchCriteria, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public ISeries[] ChartSeries
    {
        get => _chartSeries;
        set => SetProperty(ref _chartSeries, value);
    }

    public string ApartmentAddress
    {
        get => _apartmentAddress;
        set => SetProperty(ref _apartmentAddress, value);
    }

    public int ApartmentFloor
    {
        get => _apartmentFloor;
        set => SetProperty(ref _apartmentFloor, value);
    }

    public ApartmentType ApartmentType
    {
        get => _apartmentType;
        set => SetProperty(ref _apartmentType, value);
    }

    public int ApartmentSize
    {
        get => _apartmentSize;
        set => SetProperty(ref _apartmentSize, value);
    }

    public int ApartmentCapacity
    {
        get => _apartmentCapacity;
        set => SetProperty(ref _apartmentCapacity, value);
    }

    public Guid RecordApartmentId
    {
        get => _recordApartmentId;
        set => SetProperty(ref _recordApartmentId, value);
    }

    public DateTime RecordCheckIn
    {
        get => _recordCheckIn;
        set => SetProperty(ref _recordCheckIn, value);
    }

    public DateTime RecordCheckOut
    {
        get => _recordCheckOut;
        set => SetProperty(ref _recordCheckOut, value);
    }

    public double RecordDailyRate
    {
        get => _recordDailyRate;
        set => SetProperty(ref _recordDailyRate, value);
    }

    public double RecordGuestRating
    {
        get => _recordGuestRating;
        set => SetProperty(ref _recordGuestRating, value);
    }

    public OccupancyState RecordState
    {
        get => _recordState;
        set => SetProperty(ref _recordState, value);
    }

    public RelayCommand AddApartmentCommand { get; }
    public RelayCommand UpdateApartmentCommand { get; }
    public RelayCommand DeleteApartmentCommand { get; }
    public RelayCommand AddRecordCommand { get; }
    public RelayCommand UpdateRecordCommand { get; }
    public RelayCommand DeleteRecordCommand { get; }
    public RelayCommand UndoCommand { get; }
    public RelayCommand RedoCommand { get; }
    public RelayCommand SearchCommand { get; }
    public RelayCommand SaveJsonCommand { get; }
    public RelayCommand SaveXmlCommand { get; }
    public RelayCommand LoadJsonCommand { get; }
    public RelayCommand LoadXmlCommand { get; }
    public RelayCommand SimulateStateCommand { get; }
    public RelayCommand MoveNextStateCommand { get; }
    public RelayCommand ResetSearchCommand { get; }

    public void AddApartment()
    {
        try
        {
            var apartment = CreateApartmentFromForm();
            _undoRedoService.Execute(new AddApartmentCommand(_apartmentService, apartment));
            ReloadCollectionsFromServices();
            SelectedApartment = apartment;
            StatusMessage = "Apartman je uspesno dodat.";
            RefreshChart();
            RefreshCommandStates();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    public void UpdateApartment()
    {
        if (SelectedApartment is null)
        {
            return;
        }

        try
        {
            var oldValue = SelectedApartment.Clone();
            var newValue = CreateApartmentFromForm();
            newValue.Id = SelectedApartment.Id;
            _undoRedoService.Execute(new UpdateApartmentCommand(_apartmentService, oldValue, newValue));
            ReloadCollectionsFromServices();
            SelectedApartment = _apartmentService.GetAll().FirstOrDefault(a => a.Id == newValue.Id);
            StatusMessage = "Apartman je uspesno izmenjen.";
            RefreshChart();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    public void DeleteApartment()
    {
        if (SelectedApartment is null)
        {
            return;
        }

        try
        {
            var apartment = SelectedApartment.Clone();
            _undoRedoService.Execute(new DeleteApartmentCommand(_apartmentService, apartment));
            ReloadCollectionsFromServices();
            SelectedApartment = null;
            ClearApartmentForm();
            StatusMessage = "Apartman je uspesno obrisan.";
            RefreshChart();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    public void AddRecord()
    {
        try
        {
            var record = CreateRecordFromForm();
            _undoRedoService.Execute(new AddRecordCommand(_recordService, record));
            ReloadCollectionsFromServices();
            SelectedRecord = record;
            StatusMessage = "Zapis zauzetosti je uspesno dodat.";
            RefreshChart();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    public void UpdateRecord()
    {
        if (SelectedRecord is null)
        {
            return;
        }

        try
        {
            var oldValue = SelectedRecord.Clone();
            var newValue = CreateRecordFromForm();
            newValue.Id = SelectedRecord.Id;
            _undoRedoService.Execute(new UpdateRecordCommand(_recordService, oldValue, newValue));
            ReloadCollectionsFromServices();
            SelectedRecord = _recordService.GetAll().FirstOrDefault(r => r.Id == newValue.Id);
            StatusMessage = "Zapis zauzetosti je uspesno izmenjen.";
            RefreshChart();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    public void DeleteRecord()
    {
        if (SelectedRecord is null)
        {
            return;
        }

        try
        {
            var record = SelectedRecord.Clone();
            _undoRedoService.Execute(new DeleteRecordCommand(_recordService, record));
            ReloadCollectionsFromServices();
            SelectedRecord = null;
            ClearRecordForm();
            StatusMessage = "Zapis zauzetosti je uspesno obrisan.";
            RefreshChart();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    public void Undo()
    {
        _undoRedoService.Undo();
        ReloadCollectionsFromServices();
        StatusMessage = "Akcija je ponistena.";
        RefreshChart();
        RefreshCommandStates();
    }

    public void Redo()
    {
        _undoRedoService.Redo();
        ReloadCollectionsFromServices();
        StatusMessage = "Akcija je ponovo izvrsena.";
        RefreshChart();
        RefreshCommandStates();
    }

    public void Search()
    {
        var apartmentResults = _apartmentService.Search(SearchCriteria);
        var recordResults = _recordService.Search(SearchCriteria);

        Apartments.Clear();
        foreach (var apartment in apartmentResults)
        {
            Apartments.Add(apartment);
        }

        Records.Clear();
        foreach (var record in recordResults)
        {
            Records.Add(record);
        }

        StatusMessage = $"Pretraga zavrsena. Pronadjeno apartmana: {Apartments.Count}, zapisa: {Records.Count}.";
        RefreshChart();
    }

    public void ResetSearch()
    {
        SearchCriteria = string.Empty;
        ReloadCollectionsFromServices();
        StatusMessage = "Pretraga je resetovana.";
        RefreshChart();
    }

    public void Save(bool useXml)
    {
        try
        {
            if (useXml)
            {
                _persistenceService.SaveAsXml(_xmlPath);
            }
            else
            {
                _persistenceService.SaveAsJson(_jsonPath);
            }

            StatusMessage = useXml
                ? $"Podaci sacuvani u XML: {_xmlPath}"
                : $"Podaci sacuvani u JSON: {_jsonPath}";
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    public void Load(bool useXml)
    {
        try
        {
            if (useXml)
            {
                _persistenceService.LoadFromXml(_xmlPath);
            }
            else
            {
                _persistenceService.LoadFromJson(_jsonPath);
            }

            ReloadCollectionsFromServices();
            if (Apartments.Count > 0)
            {
                RecordApartmentId = Apartments[0].Id;
            }

            StatusMessage = useXml
                ? $"Podaci ucitani iz XML: {_xmlPath}"
                : $"Podaci ucitani iz JSON: {_jsonPath}";
            RefreshChart();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    public void SimulateState()
    {
        if (SelectedRecord is null)
        {
            return;
        }

        _stateSimulationService.Simulate(SelectedRecord);
        _recordService.Update(SelectedRecord);
        RecordState = SelectedRecord.State;
        StatusMessage = $"Simulacija zavrsena. Trenutno stanje: {SelectedRecord.State}.";
        RefreshChart();
    }

    public void MoveNextState()
    {
        if (SelectedRecord is null)
        {
            return;
        }

        _stateSimulationService.MoveNext(SelectedRecord);
        _recordService.Update(SelectedRecord);
        RecordState = SelectedRecord.State;
        StatusMessage = $"Novo stanje zapisa: {SelectedRecord.State}.";
        RefreshChart();
    }

    private Apartment CreateApartmentFromForm()
    {
        return new Apartment
        {
            Address = ApartmentAddress.Trim(),
            Floor = ApartmentFloor,
            Type = ApartmentType,
            Size = ApartmentSize,
            Capacity = ApartmentCapacity
        };
    }

    private ApartmentOccupancyRecord CreateRecordFromForm()
    {
        return new ApartmentOccupancyRecord
        {
            ApartmentId = RecordApartmentId,
            CheckInDate = RecordCheckIn,
            CheckoutDate = RecordCheckOut,
            DailyRate = RecordDailyRate,
            GuestRating = RecordGuestRating,
            State = RecordState
        };
    }

    private static void CopyApartmentValues(Apartment source, Apartment target)
    {
        target.Address = source.Address;
        target.Floor = source.Floor;
        target.Type = source.Type;
        target.Size = source.Size;
        target.Capacity = source.Capacity;
    }

    private static void CopyRecordValues(ApartmentOccupancyRecord source, ApartmentOccupancyRecord target)
    {
        target.ApartmentId = source.ApartmentId;
        target.CheckInDate = source.CheckInDate;
        target.CheckoutDate = source.CheckoutDate;
        target.DailyRate = source.DailyRate;
        target.GuestRating = source.GuestRating;
        target.State = source.State;
    }

    private void ReloadCollectionsFromServices()
    {
        Apartments.Clear();
        foreach (var apartment in _apartmentService.GetAll())
        {
            Apartments.Add(apartment);
        }

        Records.Clear();
        foreach (var record in _recordService.GetAll())
        {
            Records.Add(record);
        }
    }

    private void RefreshChart()
    {
        var counts = _chartService.GetRecordsCountByState();

        ChartSeries = counts
            .OrderByDescending(pair => pair.Value)
            .Select(pair => new PieSeries<int>
            {
                Name = GetStateDisplayName(pair.Key),
                Values = [pair.Value],
                Fill = new SolidColorPaint(GetStateColor(pair.Key)),
                DataLabelsPaint = new SolidColorPaint(SKColors.White),
                DataLabelsSize = 14,
                DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                DataLabelsFormatter = point => point.Coordinate.PrimaryValue.ToString()
            })
            .Cast<ISeries>()
            .ToArray();
    }

    private static string GetStateDisplayName(OccupancyState state)
    {
        return state switch
        {
            OccupancyState.Available => "Slobodan",
            OccupancyState.Reserved => "Rezervisan",
            OccupancyState.Occupied => "Zauzet",
            OccupancyState.OutOfService => "Van funkcije",
            _ => state.ToString()
        };
    }

    private static SKColor GetStateColor(OccupancyState state)
    {
        return state switch
        {
            OccupancyState.Available => SKColor.Parse("#22C55E"),
            OccupancyState.Reserved => SKColor.Parse("#F59E0B"),
            OccupancyState.Occupied => SKColor.Parse("#2E75B6"),
            OccupancyState.OutOfService => SKColor.Parse("#EF4444"),
            _ => SKColors.Gray
        };
    }

    private void RefreshCommandStates()
    {
        UndoCommand.RaiseCanExecuteChanged();
        RedoCommand.RaiseCanExecuteChanged();
    }

    private void ClearApartmentForm()
    {
        ApartmentAddress = string.Empty;
        ApartmentFloor = 0;
        ApartmentType = ApartmentType.SingleBed;
        ApartmentSize = 30;
        ApartmentCapacity = 1;
    }

    private void ClearRecordForm()
    {
        RecordApartmentId = Apartments.FirstOrDefault()?.Id ?? Guid.Empty;
        RecordCheckIn = DateTime.Today;
        RecordCheckOut = DateTime.Today.AddDays(1);
        RecordDailyRate = 1000;
        RecordGuestRating = 3;
        RecordState = OccupancyState.Available;
    }

    private void ShowError(string message)
    {
        StatusMessage = message;
        _logger.Log($"GRESKA: {message}");
        MessageBox.Show(message, "Greska", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}
