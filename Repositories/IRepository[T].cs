using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public interface IRepository<T>
	{
		ObservableCollection<T> GetAll();

		T GetById(Guid id);

		void Add(T entity);

		void Update(T entity);

		void Delete(Guid id);

		ObservableCollection<T> Search(Func<T, bool> predicate);
	}
}
