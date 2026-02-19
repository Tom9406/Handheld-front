using Handheld.ViewModels;
using Microsoft.Maui.Controls;

namespace Handheld.Views;

[QueryProperty(nameof(ShipmentId), "shipmentId")]
public partial class ShipmentLinesPage : ContentPage
{
    private readonly ShipmentLineViewModel _viewModel;

    // 🔹 Recibe el parámetro desde Shell
    public string ShipmentId
    {
        set
        {
            _viewModel.ShipmentId = Uri.UnescapeDataString(value ?? string.Empty);
        }
    }

    public ShipmentLinesPage(ShipmentLineViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!string.IsNullOrWhiteSpace(_viewModel.ShipmentId))
        {
            await _viewModel.LoadAsync();
        }
    }

    // 🔹 Método opcional si navegas manualmente sin Shell
    public async Task InitializeAsync(string shipmentId)
    {
        _viewModel.ShipmentId = shipmentId;
        await _viewModel.LoadAsync();
    }
}
    