using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public class UpdateApartmentCommand : IUndoableAction
	{
		private ApartmentService service;
		private Apartment oldValue;
		private Apartment newValue;

		public void Execute()
		{
			throw new NotImplementedException();
		}

		public void Unexecute()
		{
			throw new NotImplementedException();
		}
	}
}
