using ApartmentReservationSystem.Component1.Repositories;
using ApartmentReservationSystem.Component1.States;
using ApartmentReservationSystem.Shared.Enums;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.Services;

public class StateSimulationService
{
    private readonly LoggingService _logger;

    public StateSimulationService(LoggingService logger)
    {
        _logger = logger;
    }

    public void Simulate(ApartmentOccupancyRecord record)
    {
        var states = new[]
        {
            OccupancyState.Available,
            OccupancyState.Reserved,
            OccupancyState.Occupied,
            OccupancyState.OutOfService
        };

        foreach (var state in states)
        {
            record.State = state;
        }

        record.State = OccupancyState.Available;
        _logger.Log($"Simulirana sva stanja za zapis {record.Id}. Trenutno stanje: {record.State}");
    }

    public void MoveNext(ApartmentOccupancyRecord record)
    {
        var currentState = CreateState(record.State);
        currentState.MoveNext(record);
        _logger.Log($"Promenjeno stanje zapisa {record.Id} na {record.State}");
    }

    public OccupancyState GetCurrentState(ApartmentOccupancyRecord record) => record.State;

    public IOccupancyState CreateState(OccupancyState state)
    {
        return state switch
        {
            OccupancyState.Available => new AvailableState(),
            OccupancyState.Reserved => new ReservedState(),
            OccupancyState.Occupied => new OccupiedState(),
            OccupancyState.OutOfService => new OutOfServiceState(),
            _ => new AvailableState()
        };
    }
}
