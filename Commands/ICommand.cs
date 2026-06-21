using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public interface ICommand
	{
		bool CanExecute();

		void Execute();
	}
}
