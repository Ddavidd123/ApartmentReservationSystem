using ApartmentReservationSystem.Component1.Services;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.Commands;

public class DeleteRecordCommand : IUndoableAction
{
    private readonly OccupancyRecordService _service;
    private readonly ApartmentOccupancyRecord _record;

    public DeleteRecordCommand(OccupancyRecordService service, ApartmentOccupancyRecord record)
    {
        _service = service;
        _record = record.Clone();
    }

    public void Execute() => _service.Delete(_record.Id);

    public void Unexecute() => _service.Add(_record);
}
