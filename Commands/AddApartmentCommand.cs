using ApartmentReservationSystem.Component1.Services;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.Commands;

public class AddApartmentCommand : IUndoableAction
{
    private readonly ApartmentService _service;
    private readonly Apartment _apartment;

    public AddApartmentCommand(ApartmentService service, Apartment apartment)
    {
        _service = service;
        _apartment = apartment;
    }

    public void Execute() => _service.Add(_apartment);

    public void Unexecute() => _service.Delete(_apartment.Id);
}
