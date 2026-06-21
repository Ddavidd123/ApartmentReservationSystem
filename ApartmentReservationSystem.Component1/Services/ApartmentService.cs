using System.Collections.ObjectModel;
using System.Globalization;
using ApartmentReservationSystem.Component1.Repositories;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.Services;

public class ApartmentService
{
    private readonly IRepository<Apartment> _repository;
    private readonly LoggingService _logger;

    public ApartmentService(IRepository<Apartment> repository, LoggingService logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public bool Validate(Apartment apartment, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(apartment.Address))
        {
            errorMessage = "Adresa mora biti uneta.";
            return false;
        }

        if (apartment.Floor < 0)
        {
            errorMessage = "Sprat ne moze biti negativan.";
            return false;
        }

        if (apartment.Size <= 0)
        {
            errorMessage = "Kvadratura mora biti veca od nule.";
            return false;
        }

        if (apartment.Capacity <= 0)
        {
            errorMessage = "Kapacitet mora biti veci od nule.";
            return false;
        }

        if (!Enum.IsDefined(apartment.Type))
        {
            errorMessage = "Tip apartmana nije validan.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    public ObservableCollection<Apartment> GetAll() => _repository.GetAll();

    public void Add(Apartment apartment)
    {
        if (!Validate(apartment, out var error))
        {
            throw new InvalidOperationException(error);
        }

        if (apartment.Id == Guid.Empty)
        {
            apartment.Id = Guid.NewGuid();
        }

        _repository.Add(apartment);
        _logger.Log($"Dodat apartman: {apartment}");
    }

    public void Update(Apartment apartment)
    {
        if (!Validate(apartment, out var error))
        {
            throw new InvalidOperationException(error);
        }

        _repository.Update(apartment);
        _logger.Log($"Izmenjen apartman: {apartment}");
    }

    public void Delete(Guid id)
    {
        var apartment = _repository.GetById(id);
        _repository.Delete(id);
        _logger.Log($"Obrisan apartman: {apartment?.Address ?? id.ToString()}");
    }

    public ObservableCollection<Apartment> Search(string criteria)
    {
        if (string.IsNullOrWhiteSpace(criteria))
        {
            return GetAll();
        }

        var normalized = criteria.Trim().ToLowerInvariant();
        return _repository.Search(apartment =>
            apartment.Id.ToString().Contains(normalized, StringComparison.OrdinalIgnoreCase) ||
            apartment.Address.ToLowerInvariant().Contains(normalized) ||
            apartment.Floor.ToString(CultureInfo.InvariantCulture).Contains(normalized) ||
            apartment.Type.ToString().ToLowerInvariant().Contains(normalized) ||
            apartment.Size.ToString(CultureInfo.InvariantCulture).Contains(normalized) ||
            apartment.Capacity.ToString(CultureInfo.InvariantCulture).Contains(normalized));
    }
}
