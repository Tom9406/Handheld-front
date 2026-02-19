using Handheld.ViewModels;

namespace Handheld.Views;

public partial class ShipmentHeadersPage : ContentPage
{
    private readonly ShipmentHeadersViewModel _viewModel;

    public ShipmentHeadersPage(ShipmentHeadersViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ShipmentHeaderDto item)
        {
            ((ShipmentHeadersViewModel)BindingContext)
                .SelectShipmentCommand
                .Execute(item);

            ((CollectionView)sender).SelectedItem = null;
        }
    }

}
