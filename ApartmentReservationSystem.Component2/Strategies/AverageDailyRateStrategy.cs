using System.Globalization;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component2.Strategies;

public class AverageDailyRateStrategy : IStatisticsMethod
{
    public string GetName() => "Prosecna dnevna cena apartmana";

    public string Calculate(List<ApartmentOccupancyRecord> records)
    {
        if (records.Count == 0)
        {
            return "Nema zapisa za izracunavanje.";
        }

        var average = records.Average(record => record.DailyRate);
        return average.ToString("0.00", CultureInfo.InvariantCulture) + " RSD";
    }
}
