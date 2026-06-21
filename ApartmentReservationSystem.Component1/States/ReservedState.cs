using ApartmentReservationSystem.Shared.Enums;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.States;

public class ReservedState : IOccupancyState
{
    public OccupancyState GetStateType() => OccupancyState.Reserved;

    public void MoveNext(ApartmentOccupancyRecord record)
    {
        record.State = OccupancyState.Occupied;
    }
}
