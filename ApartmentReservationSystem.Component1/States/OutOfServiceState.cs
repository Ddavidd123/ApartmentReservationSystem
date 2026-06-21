using ApartmentReservationSystem.Shared.Enums;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.States;

public class OutOfServiceState : IOccupancyState
{
    public OccupancyState GetStateType() => OccupancyState.OutOfService;

    public void MoveNext(ApartmentOccupancyRecord record)
    {
        record.State = OccupancyState.Available;
    }
}
