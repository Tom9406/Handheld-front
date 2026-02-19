using Handheld.ViewModels;

namespace Handheld.Views;

public partial class ItemInquiryPage : ContentPage
{
    private readonly ItemInquiryViewModel _viewModel;

    public ItemInquiryPage(ItemInquiryViewModel viewModel)
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
