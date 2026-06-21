using System.Collections.ObjectModel;
using System.Globalization;
using ApartmentReservationSystem.Component1.Repositories;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.Services;

public class OccupancyRecordService
{
    private readonly IRepository<ApartmentOccupancyRecord> _repository;
    private readonly IRepository<Apartment> _apartmentRepository;
    private readonly LoggingService _logger;

    public OccupancyRecordService(
        IRepository<ApartmentOccupancyRecord> repository,
        IRepository<Apartment> apartmentRepository,
        LoggingService logger)
    {
        _repository = repository;
        _apartmentRepository = apartmentRepository;
        _logger = logger;
    }

    public bool Validate(ApartmentOccupancyRecord record, out string errorMessage)
    {
        if (_apartmentRepository.GetById(record.ApartmentId) is null)
        {
            errorMessage = "Apartman sa zadatim identifikatorom ne postoji.";
            return false;
        }

        if (record.CheckInDate >= record.CheckoutDate)
        {
            errorMessage = "Datum prijave mora biti pre datuma odjave.";
            return false;
        }

        if (record.DailyRate <= 0)
        {
            errorMessage = "Dnevna cena mora biti veca od nule.";
            return false;
        }

        if (record.GuestRating is < 1 or > 5)
        {
            errorMessage = "Ocena gosta mora biti u opsegu od 1 do 5.";
            return false;
        }

        if (!Enum.IsDefined(record.State))
        {
            errorMessage = "Stanje zapisa nije validno.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    public ObservableCollection<ApartmentOccupancyRecord> GetAll() => _repository.GetAll();

    public ObservableCollection<ApartmentOccupancyRecord> GetByApartmentAndMonth(Guid apartmentId, int month)
    {
        return new ObservableCollection<ApartmentOccupancyRecord>(
            _repository.GetAll().Where(record =>
                record.ApartmentId == apartmentId &&
                record.CheckInDate.Month == month));
    }

    public void Add(ApartmentOccupancyRecord record)
    {
        if (!Validate(record, out var error))
        {
            throw new InvalidOperationException(error);
        }

        if (record.Id == Guid.Empty)
        {
            record.Id = Guid.NewGuid();
        }

        _repository.Add(record);
        _logger.Log($"Dodat zapis zauzetosti: {record}");
    }

    public void Update(ApartmentOccupancyRecord record)
    {
        if (!Validate(record, out var error))
        {
            throw new InvalidOperationException(error);
        }

        _repository.Update(record);
        _logger.Log($"Izmenjen zapis zauzetosti: {record}");
    }

    public void Delete(Guid id)
    {
        var record = _repository.GetById(id);
        _repository.Delete(id);
        _logger.Log($"Obrisan zapis zauzetosti: {record?.Id.ToString() ?? id.ToString()}");
    }

    public ObservableCollection<ApartmentOccupancyRecord> Search(string criteria)
    {
        if (string.IsNullOrWhiteSpace(criteria))
        {
            return GetAll();
        }

        var normalized = criteria.Trim().ToLowerInvariant();
        return _repository.Search(record =>
            record.Id.ToString().Contains(normalized, StringComparison.OrdinalIgnoreCase) ||
            record.ApartmentId.ToString().Contains(normalized, StringComparison.OrdinalIgnoreCase) ||
            record.CheckInDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture).Contains(normalized) ||
            record.CheckoutDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture).Contains(normalized) ||
            record.DailyRate.ToString(CultureInfo.InvariantCulture).Contains(normalized) ||
            record.GuestRating.ToString(CultureInfo.InvariantCulture).Contains(normalized) ||
            record.State.ToString().ToLowerInvariant().Contains(normalized));
    }
}
