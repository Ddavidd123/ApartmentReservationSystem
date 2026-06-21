using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component2.Adapters;

public interface IRecordStorageAdapter
{
    void Store(List<ApartmentOccupancyRecord> records, Stores.StatisticsDataStore dataStore);
}
