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
        var counts = Enum.GetValues<OccupancyState>().ToDictionary(state => state, _ => 0);

        foreach (var record in _recordRepository.GetAll())
        {
            counts[record.State]++;
        }

        return counts;
    }

    public void RefreshChart()
    {
        ChartDataChanged?.Invoke();
    }
}
