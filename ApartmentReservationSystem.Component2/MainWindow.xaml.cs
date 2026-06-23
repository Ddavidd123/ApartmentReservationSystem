using System.Windows;
using ApartmentReservationSystem.Component2.ViewModels;

namespace ApartmentReservationSystem.Component2;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Activated += (_, _) => RefreshApartmentsIfConnected();
    }

    private void ApartmentsComboBox_DropDownOpened(object sender, EventArgs e)
    {
        RefreshApartmentsIfConnected();
    }

    private void RefreshApartmentsIfConnected()
    {
        if (DataContext is StatisticsViewModel viewModel)
        {
            viewModel.RefreshApartments(updateStatusMessage: false);
        }
    }
}
