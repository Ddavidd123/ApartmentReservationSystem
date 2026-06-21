using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public class RelayCommand : ICommand
	{
		private Action execute;
		private Func<bool> canExecute;

		public RelayCommand(Action execute, Func<bool> canExecute)
		{
			throw new NotImplementedException();
		}

		public void CanExecute()
		{
			throw new NotImplementedException();
		}

		public void Execute()
		{
			throw new NotImplementedException();
		}
	}
}
