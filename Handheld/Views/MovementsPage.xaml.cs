using Handheld.ViewModels;

namespace Handheld.Views;

public partial class MovementsPage : ContentPage
{
    private readonly InventoryMovementsViewModel _viewModel;

    public MovementsPage(InventoryMovementsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.InitializeAsync();
    }
}
