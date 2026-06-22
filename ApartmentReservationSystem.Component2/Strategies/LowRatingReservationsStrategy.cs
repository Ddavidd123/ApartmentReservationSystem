using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component2.Strategies;

public class LowRatingReservationsStrategy : IStatisticsMethod
{
    public string Name => "Broj rezervacija sa ocenom ispod 3";

    public string Calculate(List<ApartmentOccupancyRecord> records)
    {
        var count = records.Count(record => record.GuestRating < 3);
        return count.ToString();
    }
}
