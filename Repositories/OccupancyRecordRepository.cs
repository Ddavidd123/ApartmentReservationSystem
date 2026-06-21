using System.Collections.ObjectModel;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.Repositories;

public class OccupancyRecordRepository : IRepository<ApartmentOccupancyRecord>
{
    private readonly ObservableCollection<ApartmentOccupancyRecord> _items = [];

    public ObservableCollection<ApartmentOccupancyRecord> GetAll() => _items;

    public ApartmentOccupancyRecord? GetById(Guid id) => _items.FirstOrDefault(r => r.Id == id);

    public void Add(ApartmentOccupancyRecord entity) => _items.Add(entity);

    public void Update(ApartmentOccupancyRecord entity)
    {
        var existing = GetById(entity.Id);
        if (existing is null)
        {
            return;
        }

        existing.ApartmentId = entity.ApartmentId;
        existing.CheckInDate = entity.CheckInDate;
        existing.CheckoutDate = entity.CheckoutDate;
        existing.DailyRate = entity.DailyRate;
        existing.GuestRating = entity.GuestRating;
        existing.State = entity.State;
    }

    public void Delete(Guid id)
    {
        var existing = GetById(id);
        if (existing is not null)
        {
            _items.Remove(existing);
        }
    }

    public ObservableCollection<ApartmentOccupancyRecord> Search(Func<ApartmentOccupancyRecord, bool> predicate)
    {
        return new ObservableCollection<ApartmentOccupancyRecord>(_items.Where(predicate));
    }

    public void ReplaceAll(IEnumerable<ApartmentOccupancyRecord> records)
    {
        _items.Clear();
        foreach (var record in records)
        {
            _items.Add(record);
        }
    }
}
