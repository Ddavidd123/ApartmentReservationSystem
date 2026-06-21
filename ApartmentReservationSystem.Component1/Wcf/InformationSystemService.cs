using ApartmentReservationSystem.Component1.Services;
using ApartmentReservationSystem.Shared.Contracts;
using ApartmentReservationSystem.Shared.Models;
using CoreWCF;

namespace ApartmentReservationSystem.Component1.Wcf;

[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
public class InformationSystemService : IInformationSystemService
{
    private readonly ApartmentService _apartmentService;
    private readonly OccupancyRecordService _occupancyRecordService;

    public InformationSystemService(
        ApartmentService apartmentService,
        OccupancyRecordService occupancyRecordService)
    {
        _apartmentService = apartmentService;
        _occupancyRecordService = occupancyRecordService;
    }

    public List<Apartment> GetApartments()
    {
        return _apartmentService.GetAll().ToList();
    }

    public List<ApartmentOccupancyRecord> GetRecordsByApartmentAndMonth(Guid apartmentId, int month)
    {
        return _occupancyRecordService.GetByApartmentAndMonth(apartmentId, month).ToList();
    }
}
