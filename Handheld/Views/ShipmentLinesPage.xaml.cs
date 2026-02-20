using Handheld.Models;
using Handheld.ViewModels;
using Microsoft.Maui.Controls;

namespace Handheld.Views;

[QueryProperty(nameof(ShipmentId), "shipmentId")]
public partial class ShipmentLinesPage : ContentPage
{
    private readonly ShipmentLineViewModel _viewModel;
    private bool _loaded; // bandera

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

        // SOLO carga la primera vez
        if (!_loaded && !string.IsNullOrWhiteSpace(_viewModel.ShipmentId))
        {
            _loaded = true;
            await _viewModel.LoadAsync();
        }

        SearchEntry.Focus();
    }



    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var line = e.CurrentSelection.FirstOrDefault() as ShipmentLineDto;

        if (line == null)
            return;

        await Shell.Current.GoToAsync(nameof(ShipLineDetailsPage),
            new Dictionary<string, object>
            {
                ["Line"] = line
            });

        ((CollectionView)sender).SelectedItem = null;
    }

    public async Task InitializeAsync(string shipmentId)
    {
        _viewModel.ShipmentId = shipmentId;

        if (!_loaded)
        {
            _loaded = true;
            await _viewModel.LoadAsync();
        }
    }
}