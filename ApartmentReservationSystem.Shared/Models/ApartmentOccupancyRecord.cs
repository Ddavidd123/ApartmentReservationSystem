using System.Runtime.Serialization;
using ApartmentReservationSystem.Shared.Enums;

namespace ApartmentReservationSystem.Shared.Models;

[DataContract]
public class ApartmentOccupancyRecord
{
    [DataMember]
    public Guid Id { get; set; } = Guid.NewGuid();

    [DataMember]
    public Guid ApartmentId { get; set; }

    [DataMember]
    public DateTime CheckInDate { get; set; } = DateTime.Today;

    [DataMember]
    public DateTime CheckoutDate { get; set; } = DateTime.Today.AddDays(1);

    [DataMember]
    public double DailyRate { get; set; }

    [DataMember]
    public double GuestRating { get; set; } = 3;

    [DataMember]
    public OccupancyState State { get; set; } = OccupancyState.Available;

    public ApartmentOccupancyRecord Clone()
    {
        return new ApartmentOccupancyRecord
        {
            Id = Id,
            ApartmentId = ApartmentId,
            CheckInDate = CheckInDate,
            CheckoutDate = CheckoutDate,
            DailyRate = DailyRate,
            GuestRating = GuestRating,
            State = State
        };
    }

    public override string ToString()
    {
        return $"{ApartmentId} | {CheckInDate:dd.MM.yyyy} - {CheckoutDate:dd.MM.yyyy} | {DailyRate} RSD | ocena {GuestRating} | {State}";
    }
}
