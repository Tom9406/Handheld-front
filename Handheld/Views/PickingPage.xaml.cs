using Handheld.Models;
using Handheld.Services;

namespace Handheld.Views;

public partial class PickingPage : ContentPage
{
    private readonly PickService _pickService;

    // Lista completa en memoria
    private List<PickHeaderDto> _allItems = new();

    private const int PageNumber = 1;
    private const int PageSize = 20;

    public PickingPage(PickService pickService)
    {
        InitializeComponent();
        _pickService = pickService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadItems();
    }

    private async Task LoadItems()
    {
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;

        PickingList.IsVisible = false;
        ErrorLabel.IsVisible = false;

        try
        {
            var response = await _pickService.GetPicksAsync(PageNumber, PageSize);

            if (response?.Data == null || !response.Data.Any())
            {
                ErrorLabel.Text = "No hay datos";
                ErrorLabel.IsVisible = true;
                return;
            }

            _allItems = response.Data;

            PickingList.ItemsSource = _allItems;
            PickingList.IsVisible = true;
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.Message;
            ErrorLabel.IsVisible = true;
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
    }

    // =============================================
    // BUSQUEDA / SCAN (ENTER)
    // =============================================
    private void OnSearchCompleted(object sender, EventArgs e)
    {
        var text = SearchEntry.Text?.Trim();

        if (string.IsNullOrEmpty(text))
        {
            PickingList.ItemsSource = _allItems;
            return;
        }

        var filtered = _allItems
            .Where(x =>
                x.PickNo.Contains(text, StringComparison.OrdinalIgnoreCase))
            .ToList();

        PickingList.ItemsSource = filtered;
    }
}
