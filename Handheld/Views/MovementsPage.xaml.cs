using Handheld.Services;
using Handheld.Models;

namespace Handheld.Views;

public partial class MovementsPage : ContentPage
{
    private readonly MovementsService _movementService;

    // Lista completa en memoria (base para filtros)
    private List<MovementsPageDto> _allMovements = new();

    public MovementsPage(MovementsService movementService)
    {
        InitializeComponent();
        _movementService = movementService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadMovements();
    }

    private async Task LoadMovements()
    {
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;

        MovementList.IsVisible = false;
        ErrorLabel.IsVisible = false;

        try
        {
            var movements = await _movementService.GetMovementsAsync();

            if (movements == null || !movements.Any())
            {
                ErrorLabel.Text = "No hay datos";
                ErrorLabel.IsVisible = true;
                return;
            }

            // guardamos lista original
            _allMovements = movements;

            MovementList.ItemsSource = _allMovements;
            MovementList.IsVisible = true;
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

    // =====================================================
    // BUSQUEDA / SCAN (ENTER)
    // =====================================================
    private void OnSearchCompleted(object sender, EventArgs e)
    {
        var text = SearchEntry.Text?.Trim();

        // sin texto → mostrar todo
        if (string.IsNullOrEmpty(text))
        {
            MovementList.ItemsSource = _allMovements;
            return;
        }

        var filtered = _allMovements
            .Where(x =>
                (!string.IsNullOrEmpty(x.ItemNo) &&
                 x.ItemNo.Contains(text, StringComparison.OrdinalIgnoreCase))
             || (!string.IsNullOrEmpty(x.BinCode) &&
                 x.BinCode.Contains(text, StringComparison.OrdinalIgnoreCase))
             || (!string.IsNullOrEmpty(x.MovementType) &&
                 x.MovementType.Contains(text, StringComparison.OrdinalIgnoreCase))
             || (!string.IsNullOrEmpty(x.ReferenceNo) &&
                 x.ReferenceNo.Contains(text, StringComparison.OrdinalIgnoreCase))
            )
            .ToList();

        MovementList.ItemsSource = filtered;
    }
    
}
