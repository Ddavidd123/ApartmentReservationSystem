using System.Globalization;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component2.Strategies;

public class MaxRevenueStrategy : IStatisticsMethod
{
    public string GetName() => "Maksimalni prihod od apartmana";

    public string Calculate(List<ApartmentOccupancyRecord> records)
    {
        if (records.Count == 0)
        {
            return "Nema zapisa za izracunavanje.";
        }

        var maxRevenue = records
            .Select(record => record.DailyRate * (record.CheckoutDate - record.CheckInDate).Days)
            .DefaultIfEmpty(0)
            .Max();

        return maxRevenue.ToString("0.00", CultureInfo.InvariantCulture) + " RSD";
    }
}
