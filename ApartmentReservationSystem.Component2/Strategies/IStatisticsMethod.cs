using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component2.Strategies;

public interface IStatisticsMethod
{
    string Name { get; }
    string Calculate(List<ApartmentOccupancyRecord> records);
}
