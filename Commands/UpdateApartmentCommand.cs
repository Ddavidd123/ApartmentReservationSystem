using ApartmentReservationSystem.Component1.Services;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.Commands;

public class UpdateApartmentCommand : IUndoableAction
{
    private readonly ApartmentService _service;
    private readonly Apartment _oldValue;
    private readonly Apartment _newValue;

    public UpdateApartmentCommand(ApartmentService service, Apartment oldValue, Apartment newValue)
    {
        _service = service;
        _oldValue = oldValue.Clone();
        _newValue = newValue.Clone();
    }

    public void Execute() => _service.Update(_newValue);

    public void Unexecute() => _service.Update(_oldValue);
}
