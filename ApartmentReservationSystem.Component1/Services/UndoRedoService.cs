using ApartmentReservationSystem.Component1.Commands;

namespace ApartmentReservationSystem.Component1.Services;

public class UndoRedoService
{
    private readonly Stack<IUndoableAction> _undoStack = new();
    private readonly Stack<IUndoableAction> _redoStack = new();

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;

    public void Execute(IUndoableAction action)
    {
        action.Execute();
        _undoStack.Push(action);
        _redoStack.Clear();
    }

    public void Undo()
    {
        if (!CanUndo)
        {
            return;
        }

        var action = _undoStack.Pop();
        action.Unexecute();
        _redoStack.Push(action);
    }

    public void Redo()
    {
        if (!CanRedo)
        {
            return;
        }

        var action = _redoStack.Pop();
        action.Execute();
        _undoStack.Push(action);
    }
}
