using ApartmentReservationSystem.Component1.Services;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.Commands;

public class UpdateRecordCommand : IUndoableAction
{
    private readonly OccupancyRecordService _service;
    private readonly ApartmentOccupancyRecord _oldValue;
    private readonly ApartmentOccupancyRecord _newValue;

    public UpdateRecordCommand(
        OccupancyRecordService service,
        ApartmentOccupancyRecord oldValue,
        ApartmentOccupancyRecord newValue)
    {
        _service = service;
        _oldValue = oldValue.Clone();
        _newValue = newValue.Clone();
    }

    public void Execute() => _service.Update(_newValue);

    public void Unexecute() => _service.Update(_oldValue);
}
