using Handheld.Views;
using System.Windows.Input;

namespace Handheld;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("PickingPage", typeof(PickingPage));
        Routing.RegisterRoute("PICKING_TAKE", typeof(PICKING_TAKE));
        Routing.RegisterRoute("Picking_Line_Details", typeof(Picking_Line_Details));
        Routing.RegisterRoute("ItemInquiryPage", typeof(ItemInquiryPage));
        Routing.RegisterRoute("MovementsPage", typeof(MovementsPage));
        Routing.RegisterRoute("ReceivingPage", typeof(ReceivingPage));
        Routing.RegisterRoute("ShipmentHeadersPage", typeof(ShipmentHeadersPage));
        Routing.RegisterRoute("RegisterCompanyPage", typeof(RegisterCompanyPage));
        Routing.RegisterRoute("MainPage", typeof(MainPage));
        Routing.RegisterRoute("ShipmentLinesPage", typeof(ShipmentLinesPage));
        Routing.RegisterRoute(nameof(ShipLineDetailsPage), typeof(ShipLineDetailsPage));
        Routing.RegisterRoute(nameof(ReceivingLinesPage), typeof(ReceivingLinesPage));
        Routing.RegisterRoute(nameof(ReceivingLineDetailsPage), typeof(ReceivingLineDetailsPage));
    }

    private async void OnPostClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//items");
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopToRootAsync(false); 
        await Shell.Current.GoToAsync("//home", true );              
    }

    public ICommand GoReceiveCommand => new Command(async () =>
    {
        await Shell.Current.GoToAsync("receiving"); 
    });

    private async void OnShipmentClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//shipments");
    }

    private async void OnCompanyClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegisterCompanyPage)); 
    }


    protected override bool OnBackButtonPressed()
    {
        if (Shell.Current.Navigation.NavigationStack.Count > 1)
            return base.OnBackButtonPressed();

        return true; // bloquea cierre
    }


}

