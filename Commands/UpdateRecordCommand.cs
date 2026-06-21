using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public class UpdateRecordCommand : IUndoableAction
	{
		private OccupancyRecordService service;
		private ApartmentOccupancyRecord oldValue;
		private ApartmentOccupancyRecord newValue;

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
