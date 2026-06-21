using System.Runtime.Serialization;

namespace ApartmentReservationSystem.Shared.Models;

[DataContract]
public class PersistenceData
{
    [DataMember]
    public List<Apartment> Apartments { get; set; } = [];

    [DataMember]
    public List<ApartmentOccupancyRecord> Records { get; set; } = [];
}
