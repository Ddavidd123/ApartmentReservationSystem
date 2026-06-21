using System.IO;

namespace ApartmentReservationSystem.Component2.Services;

public class CsvExportService
{
    public void Export(string result, string path)
    {
        File.WriteAllText(path, result);
    }
}
