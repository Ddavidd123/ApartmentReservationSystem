using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public interface IUndoableAction
	{
		void Execute();

		void Unexecute();
	}
}
