using System.Runtime.Serialization;
using ApartmentReservationSystem.Shared.Enums;

namespace ApartmentReservationSystem.Shared.Models;

[DataContract]
public class Apartment
{
    [DataMember]
    public Guid Id { get; set; } = Guid.NewGuid();

    [DataMember]
    public string Address { get; set; } = string.Empty;

    [DataMember]
    public int Floor { get; set; }

    [DataMember]
    public ApartmentType Type { get; set; }

    [DataMember]
    public int Size { get; set; }

    [DataMember]
    public int Capacity { get; set; }

    public Apartment Clone()
    {
        return new Apartment
        {
            Id = Id,
            Address = Address,
            Floor = Floor,
            Type = Type,
            Size = Size,
            Capacity = Capacity
        };
    }

    public override string ToString()
    {
        return $"{Address} | sprat {Floor} | {Type} | {Size}m2 | kapacitet {Capacity}";
    }
}
