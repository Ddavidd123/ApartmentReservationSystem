using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using ApartmentReservationSystem.Component1.Repositories;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.Services;

public class PersistenceService
{
    private readonly ApartmentRepository _apartmentRepository;
    private readonly OccupancyRecordRepository _recordRepository;
    private readonly LoggingService _logger;

    public PersistenceService(
        ApartmentRepository apartmentRepository,
        OccupancyRecordRepository recordRepository,
        LoggingService logger)
    {
        _apartmentRepository = apartmentRepository;
        _recordRepository = recordRepository;
        _logger = logger;
    }

    public void SaveAsXml(string path)
    {
        var data = CreateSnapshot();
        var serializer = new XmlSerializer(typeof(PersistenceData));
        using var stream = File.Create(path);
        serializer.Serialize(stream, data);
        _logger.Log($"Podaci sacuvani u XML fajl: {path}");
    }

    public void LoadFromXml(string path)
    {
        if (!File.Exists(path))
        {
            SeedDefaultData();
            SaveAsXml(path);
            return;
        }

        var serializer = new XmlSerializer(typeof(PersistenceData));
        using var stream = File.OpenRead(path);
        if (serializer.Deserialize(stream) is PersistenceData data)
        {
            ApplySnapshot(data);
        }

        EnsureMinimumData();
        _logger.Log($"Podaci ucitani iz XML fajla: {path}");
    }

    public void SaveAsJson(string path)
    {
        var data = CreateSnapshot();
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
        _logger.Log($"Podaci sacuvani u JSON fajl: {path}");
    }

    public void LoadFromJson(string path)
    {
        if (!File.Exists(path))
        {
            SeedDefaultData();
            SaveAsJson(path);
            return;
        }

        var json = File.ReadAllText(path);
        var data = JsonSerializer.Deserialize<PersistenceData>(json);
        if (data is not null)
        {
            ApplySnapshot(data);
        }

        EnsureMinimumData();
        _logger.Log($"Podaci ucitani iz JSON fajla: {path}");
    }

    public void SeedDefaultData()
    {
        _apartmentRepository.ReplaceAll(SampleDataFactory.CreateApartments());
        _recordRepository.ReplaceAll(SampleDataFactory.CreateRecords(_apartmentRepository.GetAll()));
        _logger.Log("Ucitani podrazumevani primer podataka.");
    }

    private PersistenceData CreateSnapshot()
    {
        return new PersistenceData
        {
            Apartments = _apartmentRepository.GetAll().ToList(),
            Records = _recordRepository.GetAll().ToList()
        };
    }

    private void ApplySnapshot(PersistenceData data)
    {
        _apartmentRepository.ReplaceAll(data.Apartments);
        _recordRepository.ReplaceAll(data.Records);
    }

    private void EnsureMinimumData()
    {
        if (_apartmentRepository.GetAll().Count >= 3 && _recordRepository.GetAll().Count >= 3)
        {
            return;
        }

        SeedDefaultData();
    }
}

public static class SampleDataFactory
{
    public static List<Apartment> CreateApartments()
    {
        return
        [
            new Apartment
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Address = "Knez Mihailova 12 4",
                Floor = 2,
                Type = Shared.Enums.ApartmentType.Studio,
                Size = 35,
                Capacity = 2
            },
            new Apartment
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Address = "Bulevar Kralja Aleksandra 45 12",
                Floor = 5,
                Type = Shared.Enums.ApartmentType.DoubleBed,
                Size = 52,
                Capacity = 4
            },
            new Apartment
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Address = "Narodnog Fronta 8 1",
                Floor = 0,
                Type = Shared.Enums.ApartmentType.SingleBed,
                Size = 28,
                Capacity = 1
            }
        ];
    }

    public static List<ApartmentOccupancyRecord> CreateRecords(ObservableCollection<Apartment> apartments)
    {
        var first = apartments[0].Id;
        var second = apartments[1].Id;
        var third = apartments[2].Id;

        return
        [
            new ApartmentOccupancyRecord
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                ApartmentId = first,
                CheckInDate = new DateTime(DateTime.Today.Year, 5, 13),
                CheckoutDate = new DateTime(DateTime.Today.Year, 5, 17),
                DailyRate = 1500,
                GuestRating = 4.2,
                State = Shared.Enums.OccupancyState.Occupied
            },
            new ApartmentOccupancyRecord
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                ApartmentId = second,
                CheckInDate = new DateTime(DateTime.Today.Year, 6, 12),
                CheckoutDate = new DateTime(DateTime.Today.Year, 6, 17),
                DailyRate = 2800,
                GuestRating = 3.7,
                State = Shared.Enums.OccupancyState.Available
            },
            new ApartmentOccupancyRecord
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                ApartmentId = third,
                CheckInDate = new DateTime(DateTime.Today.Year, 7, 1),
                CheckoutDate = new DateTime(DateTime.Today.Year, 7, 10),
                DailyRate = 1200,
                GuestRating = 2.5,
                State = Shared.Enums.OccupancyState.Reserved
            }
        ];
    }
}
