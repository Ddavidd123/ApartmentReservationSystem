using ApartmentReservationSystem.Component2.Stores;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component2.Adapters;

public class DictionaryRecordStorageAdapter : IRecordStorageAdapter
{
    public void Store(List<ApartmentOccupancyRecord> records, StatisticsDataStore dataStore)
    {
        foreach (var record in records)
        {
            var key = BuildKey(record);
            dataStore.Add(key, [record]);
        }
    }

    private static string BuildKey(ApartmentOccupancyRecord record)
    {
        return $"{record.ApartmentId}-{record.CheckInDate:yyyy-MM-dd}";
    }
}
