using System.Globalization;
using System.Windows.Data;
using ApartmentReservationSystem.Shared.Models;

namespace ApartmentReservationSystem.Component1.Helpers;

public class ApartmentIdToAddressConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2 ||
            values[0] is not Guid apartmentId ||
            values[1] is not IEnumerable<Apartment> apartments)
        {
            return string.Empty;
        }

        var apartment = apartments.FirstOrDefault(a => a.Id == apartmentId);
        return apartment?.Address ?? apartmentId.ToString()[..8] + "...";
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
