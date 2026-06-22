using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component2.Stores;

public class StatisticsDataStore
{
    private readonly Dictionary<string, List<ApartmentOccupancyRecord>> _recordsByKey = [];

    public void Clear()
    {
        _recordsByKey.Clear();
    }

    public void AddOrAppend(string key, ApartmentOccupancyRecord record)
    {
        if (!_recordsByKey.TryGetValue(key, out var records))
        {
            records = [];
            _recordsByKey[key] = records;
        }

        records.Add(record);
    }

    public List<ApartmentOccupancyRecord> Get(string key)
    {
        return _recordsByKey.TryGetValue(key, out var records)
            ? records
            : [];
    }

    public IReadOnlyDictionary<string, List<ApartmentOccupancyRecord>> GetAllEntries()
    {
        return _recordsByKey;
    }
}
