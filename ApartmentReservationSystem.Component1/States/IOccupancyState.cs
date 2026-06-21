using ApartmentReservationSystem.Shared.Enums;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.States;

public interface IOccupancyState
{
    OccupancyState GetStateType();
    void MoveNext(ApartmentOccupancyRecord record);
}
