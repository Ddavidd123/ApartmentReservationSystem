namespace ApartmentReservationSystem.Component1.Commands;

public interface IUndoableAction
{
    void Execute();
    void Unexecute();
}
