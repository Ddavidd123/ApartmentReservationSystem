using System.Globalization;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component2.Helpers;

public static class RecordDisplayFormatter
{
    public static string Format(ApartmentOccupancyRecord record)
    {
        var checkIn = record.CheckInDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        var checkOut = record.CheckoutDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        var rating = record.GuestRating.ToString("0.#", CultureInfo.InvariantCulture);

        return $"({record.ApartmentId}, {checkIn}. - {checkOut}.) -> " +
               $"[dailyRate: {record.DailyRate} RSD, rating: {rating}, state: {record.State}]";
    }
}
