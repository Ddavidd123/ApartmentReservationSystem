using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public class UndoRedoService
	{
		private Stack<IUndoableAction> undoStack;
		private Stack<IUndoableAction> redoStack;

		public void Execute(IUndoableAction action)
		{
			throw new NotImplementedException();
		}

		public void Undo()
		{
			throw new NotImplementedException();
		}

		public void Redo()
		{
			throw new NotImplementedException();
		}
	}
}
