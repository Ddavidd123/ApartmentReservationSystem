using ApartmentReservationSystem.Component2.Stores;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component2.Adapters;

public class DictionaryRecordStorageAdapter : IRecordStorageAdapter
{
    public void Store(List<ApartmentOccupancyRecord> records, StatisticsDataStore dataStore)
    {
        dataStore.Clear();

        foreach (var record in records)
        {
            dataStore.AddOrAppend(BuildKey(record), record);
        }
    }

    internal static string BuildKey(ApartmentOccupancyRecord record)
    {
        return $"{record.ApartmentId}-{record.CheckInDate:yyyy-MM-dd}";
    }
}
