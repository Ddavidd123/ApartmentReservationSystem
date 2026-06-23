using System.Globalization;
using System.IO;
using System.Text;
using ApartmentReservationSystem.Component2.Helpers;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component2.Services;

public class CsvExportService
{
    public void Export(
        IReadOnlyList<ApartmentOccupancyRecord> records,
        string? methodName,
        string? result,
        string path)
    {
        var builder = new StringBuilder();

        builder.AppendLine("Preuzeti zapisi");
        builder.AppendLine("ApartmentId,CheckInDate,CheckOutDate,DailyRate,GuestRating,State,Prikaz");

        foreach (var record in records)
        {
            builder.Append(Escape(record.ApartmentId.ToString()));
            builder.Append(',');
            builder.Append(Escape(record.CheckInDate.ToString("dd.MM.yyyy.", CultureInfo.InvariantCulture)));
            builder.Append(',');
            builder.Append(Escape(record.CheckoutDate.ToString("dd.MM.yyyy.", CultureInfo.InvariantCulture)));
            builder.Append(',');
            builder.Append(Escape(record.DailyRate.ToString(CultureInfo.InvariantCulture)));
            builder.Append(',');
            builder.Append(Escape(record.GuestRating.ToString("0.#", CultureInfo.InvariantCulture)));
            builder.Append(',');
            builder.Append(Escape(record.State.ToString()));
            builder.Append(',');
            builder.AppendLine(Escape(RecordDisplayFormatter.Format(record)));
        }

        builder.AppendLine();
        builder.AppendLine("Statisticka analiza");
        builder.AppendLine("Metoda,Rezultat");
        builder.Append(Escape(methodName ?? string.Empty));
        builder.Append(',');
        builder.AppendLine(Escape(result ?? string.Empty));

        File.WriteAllText(path, builder.ToString(), Encoding.UTF8);
    }

    private static string Escape(string value)
    {
        return $"\"{value.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";
    }
}
