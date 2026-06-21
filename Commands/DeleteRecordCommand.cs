using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public class DeleteRecordCommand : IUndoableAction
	{
		private OccupancyRecordService service;
		private ApartmentOccupancyRecord record;

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
