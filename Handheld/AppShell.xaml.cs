using Handheld.Views;

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
        await Shell.Current.GoToAsync(nameof(ItemInquiryPage));
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MainPage));
    }

    private async void OnReceivingClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ReceivingPage));
    }

    private async void OnShipmentClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ShipmentHeadersPage)); 
    }

    private async void OnCompanyClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegisterCompanyPage)); 
    }


}

