using Handheld.Models;
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

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var header = e.CurrentSelection.FirstOrDefault() as ReceivingHeaderDto;

        if (header == null)
            return;

        await Shell.Current.GoToAsync(
            $"ReceivingLinesPage?headerId={header.Id}");

        ((CollectionView)sender).SelectedItem = null;
    }
}