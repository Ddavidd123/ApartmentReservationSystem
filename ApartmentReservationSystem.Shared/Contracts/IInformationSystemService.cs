using System.ServiceModel;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Shared.Contracts;

[ServiceContract]
public interface IInformationSystemService
{
    [OperationContract]
    List<Apartment> GetApartments();

    [OperationContract]
    List<ApartmentOccupancyRecord> GetRecordsByApartmentAndMonth(Guid apartmentId, int month);
}
