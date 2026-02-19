using Handheld.ViewModels;

namespace Handheld.Views;

public partial class ReceivingPage : ContentPage
{
    private readonly ReceivingHeadersViewModel _viewModel;

    public ReceivingPage(ReceivingHeadersViewModel viewModel)
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
}
