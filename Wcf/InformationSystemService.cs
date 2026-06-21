using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public class InformationSystemService : IInformationSystemService
	{
		private ApartmentService apartmentService;
		private OccupancyRecordService occupancyRecordService;

		public List<Apartment> GetApartments()
		{
			throw new NotImplementedException();
		}

		public List<ApartmentOccupancyRecord> GetRecordsByApartmentAndMonth(Guid apartmentId, int month)
		{
			throw new NotImplementedException();
		}
	}
}
