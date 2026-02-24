using Handheld.Models;
using Handheld.ViewModels;
using Microsoft.Maui.Controls;

namespace Handheld.Views;

[QueryProperty(nameof(HeaderId), "headerId")]
public partial class ReceivingLinesPage : ContentPage
{
    private readonly ReceivingLineViewModel _viewModel;
    private bool _loaded;

    public string HeaderId
    {
        set
        {
            if (Guid.TryParse(
                Uri.UnescapeDataString(value ?? string.Empty),
                out var id))
            {
                _viewModel.ReceivingHeaderId = id;
            }
        }
    }

    public ReceivingLinesPage(ReceivingLineViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.ReceivingHeaderId != Guid.Empty)
        {
            await _viewModel.LoadAsync();
        }
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var line = e.CurrentSelection.FirstOrDefault() as ReceivingLineDto;

        if (line == null)
            return;

        await Shell.Current.GoToAsync(nameof(ReceivingLineDetailsPage),
            new Dictionary<string, object>
            {
                ["Line"] = line
            });

        ((CollectionView)sender).SelectedItem = null;
    }

    public async Task InitializeAsync(Guid headerId)
    {
        _viewModel.ReceivingHeaderId = headerId;

        if (!_loaded)
        {
            _loaded = true;
            await _viewModel.LoadAsync();
        }
    }
}