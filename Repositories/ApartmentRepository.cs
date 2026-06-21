using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public class ApartmentRepository : IRepository<T>
	{
		private ObservableCollection<Apartment> items;

		public ObservableCollection<Apartment> GetAll()
		{
			throw new NotImplementedException();
		}

		public Apartment GetById(Guid id)
		{
			throw new NotImplementedException();
		}

		public void Add(Apartment entity)
		{
			throw new NotImplementedException();
		}

		public void Update(Apartment entity)
		{
			throw new NotImplementedException();
		}

		public void Delete(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
