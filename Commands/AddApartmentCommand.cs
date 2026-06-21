using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public class AddApartmentCommand : IUndoableAction
	{
		private ApartmentService service;
		private Apartment apartment;

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
