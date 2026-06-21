using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public class OccupancyRecordRepository : IRepository<T>
	{
		private ObservableCollection<ApartmentOccupancyRecord> items;

		public ObservableCollection<ApartmentOccupancyRecord> GetAll()
		{
			throw new NotImplementedException();
		}

		public ApartmentOccupancyRecord GetById(Guid id)
		{
			throw new NotImplementedException();
		}

		public void Add(ApartmentOccupancyRecord entity)
		{
			throw new NotImplementedException();
		}

		public void Update(ApartmentOccupancyRecord entity)
		{
			throw new NotImplementedException();
		}

		public void Delete(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
