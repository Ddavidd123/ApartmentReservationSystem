using ApartmentReservationSystem.Shared.Enums;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.States;

public class OccupiedState : IOccupancyState
{
    public OccupancyState GetStateType() => OccupancyState.Occupied;

    public void MoveNext(ApartmentOccupancyRecord record)
    {
        record.State = OccupancyState.OutOfService;
    }
}
