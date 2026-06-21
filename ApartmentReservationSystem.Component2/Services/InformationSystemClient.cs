using ApartmentReservationSystem.Shared.Contracts;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component2.Services;

public class InformationSystemClient
{
    private readonly IInformationSystemService _service;

    public InformationSystemClient(IInformationSystemService service)
    {
        _service = service;
    }

    public List<ApartmentOccupancyRecord> RequestRecords(Guid apartmentId, int month)
    {
        return _service.GetRecordsByApartmentAndMonth(apartmentId, month);
    }
}
