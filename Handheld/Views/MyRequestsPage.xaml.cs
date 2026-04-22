using Handheld.Models;
using Handheld.ViewModels;

namespace Handheld.Views;

public partial class MyRequestsPage : ContentPage
{
    private readonly MyRequestsViewModel _vm;

    public MyRequestsPage(MyRequestsViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadRequests();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//home");
    }
    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        var item = e.CurrentSelection.FirstOrDefault() as ServiceRequestDto;

        if (item == null)
            return;

        try
        {
            await Shell.Current.GoToAsync("requestdetail",
    new Dictionary<string, object>
    {
        ["id"] = item.RequestID
    });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error navegación", ex.Message, "OK");
        }

        ((CollectionView)sender).SelectedItem = null;
    }
}
