using ApartmentReservationSystem.Component1.Services;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.Commands;

public class AddRecordCommand : IUndoableAction
{
    private readonly OccupancyRecordService _service;
    private readonly ApartmentOccupancyRecord _record;

    public AddRecordCommand(OccupancyRecordService service, ApartmentOccupancyRecord record)
    {
        _service = service;
        _record = record;
    }

    public void Execute() => _service.Add(_record);

    public void Unexecute() => _service.Delete(_record.Id);
}
