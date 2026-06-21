using System.Collections.ObjectModel;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.Repositories;

public class ApartmentRepository : IRepository<Apartment>
{
    private readonly ObservableCollection<Apartment> _items = [];

    public ObservableCollection<Apartment> GetAll() => _items;

    public Apartment? GetById(Guid id) => _items.FirstOrDefault(a => a.Id == id);

    public void Add(Apartment entity) => _items.Add(entity);

    public void Update(Apartment entity)
    {
        var existing = GetById(entity.Id);
        if (existing is null)
        {
            return;
        }

        existing.Address = entity.Address;
        existing.Floor = entity.Floor;
        existing.Type = entity.Type;
        existing.Size = entity.Size;
        existing.Capacity = entity.Capacity;
    }

    public void Delete(Guid id)
    {
        var existing = GetById(id);
        if (existing is not null)
        {
            _items.Remove(existing);
        }
    }

    public ObservableCollection<Apartment> Search(Func<Apartment, bool> predicate)
    {
        return new ObservableCollection<Apartment>(_items.Where(predicate));
    }

    public void ReplaceAll(IEnumerable<Apartment> apartments)
    {
        _items.Clear();
        foreach (var apartment in apartments)
        {
            _items.Add(apartment);
        }
    }
}
