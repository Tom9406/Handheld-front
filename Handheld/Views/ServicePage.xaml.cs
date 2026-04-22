using Handheld.Models;
using Handheld.ViewModels;

namespace Handheld.Views;

[QueryProperty(nameof(Category), "category")]
public partial class ServicesPage : ContentPage
{
    private string? _category;
    private readonly ServicesViewModel _vm;

    public string? Category
    {
        get => _category;
        set
        {
            _category = value;

            if (!string.IsNullOrWhiteSpace(_category))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await _vm.LoadServicesByCategory(_category);
                });
            }
        }
    }

    public ServicesPage(ServicesViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//home");
    }

    private async void OnServiceSelected(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            var service = e.CurrentSelection.FirstOrDefault() as ServiceDto;

            if (service == null)
                return;

            await Shell.Current.GoToAsync("mynewrequest", new Dictionary<string, object>
            {
                ["serviceId"] = service.ServiceID
            });

            ((CollectionView)sender).SelectedItem = null;
        }
        catch (Exception ex)
        {
            await DisplayAlert("ERROR", ex.ToString(), "OK");
        }
    }
}
