using ApartmentReservationSystem.Component1.Services;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.Commands;

public class DeleteApartmentCommand : IUndoableAction
{
    private readonly ApartmentService _service;
    private readonly Apartment _apartment;

    public DeleteApartmentCommand(ApartmentService service, Apartment apartment)
    {
        _service = service;
        _apartment = apartment.Clone();
    }

    public void Execute() => _service.Delete(_apartment.Id);

    public void Unexecute() => _service.Add(_apartment);
}
