using System.Collections.ObjectModel;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.Repositories;

public interface IRepository<T>
{
    ObservableCollection<T> GetAll();
    T? GetById(Guid id);
    void Add(T entity);
    void Update(T entity);
    void Delete(Guid id);
    ObservableCollection<T> Search(Func<T, bool> predicate);
}
