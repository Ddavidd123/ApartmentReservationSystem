using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public interface IOccupancyState
	{
		OccupancyState GetStateType();

		void MoveNext(ApartmentOccupancyRecord record);
	}
}
