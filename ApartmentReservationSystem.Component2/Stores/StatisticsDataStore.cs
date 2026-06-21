using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component2.Stores;

public class StatisticsDataStore
{
    private readonly Dictionary<string, List<ApartmentOccupancyRecord>> _recordsByApartmentMonth = [];

    public void Add(string key, List<ApartmentOccupancyRecord> records)
    {
        _recordsByApartmentMonth[key] = records;
    }

    public List<ApartmentOccupancyRecord> Get(string key)
    {
        return _recordsByApartmentMonth.TryGetValue(key, out var records)
            ? records
            : [];
    }

    public IReadOnlyDictionary<string, List<ApartmentOccupancyRecord>> GetAllEntries()
    {
        return _recordsByApartmentMonth;
    }
}
