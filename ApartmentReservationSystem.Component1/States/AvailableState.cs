using ApartmentReservationSystem.Shared.Enums;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.States;

public class AvailableState : IOccupancyState
{
    public OccupancyState GetStateType() => OccupancyState.Available;

    public void MoveNext(ApartmentOccupancyRecord record)
    {
        record.State = OccupancyState.Reserved;
    }
}
