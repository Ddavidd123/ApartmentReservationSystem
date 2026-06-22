using ApartmentReservationSystem.Component1.Repositories;
using ApartmentReservationSystem.Shared.Enums;

namespace ApartmentReservationSystem.Component1.Services;

public class ChartService
{
    private readonly OccupancyRecordRepository _recordRepository;
    public event Action? ChartDataChanged;

    public ChartService(OccupancyRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
        _recordRepository.GetAll().CollectionChanged += (_, _) => RefreshChart();
    }

    public Dictionary<OccupancyState, int> GetRecordsCountByState()
    {
        return _recordRepository.GetAll()
            .GroupBy(record => record.State)
            .ToDictionary(group => group.Key, group => group.Count());
    }

    public void RefreshChart()
    {
        ChartDataChanged?.Invoke();
    }
}
