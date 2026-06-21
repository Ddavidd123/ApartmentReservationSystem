using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public class OccupancyRecordService
	{
		private IRepository<ApartmentOccupancyRecord> repository;
		private LoggingService logger;

		public bool Validate(ApartmentOccupancyRecord record)
		{
			throw new NotImplementedException();
		}

		public ObservableCollection<ApartmentOccupancyRecord> GetAll()
		{
			throw new NotImplementedException();
		}

		public ObservableCollection<ApartmentOccupancyRecord> GetByApartmentAndMonth(Guid apartmentId, int month)
		{
			throw new NotImplementedException();
		}

		public void Add(ApartmentOccupancyRecord record)
		{
			throw new NotImplementedException();
		}

		public void Update(ApartmentOccupancyRecord record)
		{
			throw new NotImplementedException();
		}

		public void Delete(Guid id)
		{
			throw new NotImplementedException();
		}

		public ObservableCollection<ApartmentOccupancyRecord> Search(string criteria)
		{
			throw new NotImplementedException();
		}
	}
}
