using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component2.Strategies;

public interface IStatisticsMethod
{
    string GetName();
    string Calculate(List<ApartmentOccupancyRecord> records);
}
