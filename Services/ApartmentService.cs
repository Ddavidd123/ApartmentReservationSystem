using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public class ApartmentService
	{
		private IRepository<Apartment> repository;
		private LoggingService logger;

		public bool Validate(Apartment apartment)
		{
			throw new NotImplementedException();
		}

		public ObservableCollection<Apartment> GetAll()
		{
			throw new NotImplementedException();
		}

		public void Add(Apartment apartment)
		{
			throw new NotImplementedException();
		}

		public void Update(Apartment apartment)
		{
			throw new NotImplementedException();
		}

		public void Delete(Guid id)
		{
			throw new NotImplementedException();
		}

		public ObservableCollection<Apartment> Search(string criteria)
		{
			throw new NotImplementedException();
		}
	}
}
