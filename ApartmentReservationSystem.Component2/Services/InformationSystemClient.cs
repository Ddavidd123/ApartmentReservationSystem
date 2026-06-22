using ApartmentReservationSystem.Shared.Contracts;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component2.Services;

public class InformationSystemClient
{
    private readonly InformationSystemConnection _connection;

    public InformationSystemClient(InformationSystemConnection connection)
    {
        _connection = connection;
    }

    public bool IsConnected => _connection.IsConnected;

    public List<Apartment> GetApartments()
    {
        EnsureConnected();
        return _connection.Service!.GetApartments();
    }

    public List<ApartmentOccupancyRecord> RequestRecords(Guid apartmentId, int month)
    {
        EnsureConnected();
        return _connection.Service!.GetRecordsByApartmentAndMonth(apartmentId, month);
    }

    private void EnsureConnected()
    {
        if (!_connection.IsConnected)
        {
            throw new InvalidOperationException(
                "Nema konekcije sa Komponentom 1. Pokrenite informacioni sistem i ponovo pokrenite ovu aplikaciju.");
        }
    }
}
