using System.IO;

namespace ApartmentReservationSystem.Component1.Services;

public class LoggingService
{
    private readonly string _logFilePath;

    public LoggingService(string logFilePath)
    {
        _logFilePath = logFilePath;
        Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath)!);
    }

    public void Log(string message)
    {
        var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}";
        File.AppendAllText(_logFilePath, line + Environment.NewLine);
    }
}
