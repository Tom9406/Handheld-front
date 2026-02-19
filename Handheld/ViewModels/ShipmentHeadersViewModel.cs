using Handheld.Models;
using Handheld.Services;
using Handheld.ViewModels.Base;
using Handheld.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Handheld.ViewModels;

public class ShipmentHeadersViewModel : BaseViewModel
{
    private readonly ShipmentService _shipmentService;

    private const string CompanyId = "FC73E7BF-C62D-48FF-AC17-18244D67DFE4";
    private const int PageNumber = 1;
    private const int PageSize = 20;

    private List<ShipmentHeaderDto> _allItems = new();

    public ObservableCollection<ShipmentHeaderDto> Items { get; } = new();

    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                FilterShipments();
            }
        }
    }

    public bool HasData => Items.Count > 0;

    public override bool IsEmpty => !IsLoading && !HasError && !HasData;

    public ICommand LoadCommand { get; }
    public ICommand SelectShipmentCommand { get; }

    public ShipmentHeadersViewModel(ShipmentService shipmentService)
    {
        _shipmentService = shipmentService;

        LoadCommand = new Command(async () => await LoadAsync());

        SelectShipmentCommand = new Command<ShipmentHeaderDto>(async (item) =>
            await OpenShipmentAsync(item));

        Items.CollectionChanged += (_, __) =>
        {
            OnPropertyChanged(nameof(HasData));
            OnPropertyChanged(nameof(IsEmpty));
        };
    }

    private void FilterShipments()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            Items.Clear();
            foreach (var item in _allItems)
                Items.Add(item);
            return;
        }

        var search = SearchText.Trim();

        var filtered = _allItems
            .Where(x =>
                (x.ShipmentNo?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.ExternalShipmentNo?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.ShipmentType?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.ShipmentStatus?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.WarehouseCode?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.CustomerCode?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.CustomerName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.CompanyCode?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                x.TotalLines.ToString().Contains(search) ||
                x.TotalQty.ToString().Contains(search) ||
                (x.IsClosed ? "Closed" : "Open")
                    .Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (x.PlannedShipDate?.ToString("yyyy-MM-dd").Contains(search) ?? false) ||
                (x.ActualShipDate?.ToString("yyyy-MM-dd").Contains(search) ?? false) ||
                x.CreatedAt.ToString("yyyy-MM-dd").Contains(search)
            )
            .ToList();

        Items.Clear();
        foreach (var item in filtered)
            Items.Add(item);
    }

    private async Task OpenShipmentAsync(ShipmentHeaderDto item)
    {
        if (item == null)
            return;

        await Shell.Current.GoToAsync(
            $"{nameof(ShipmentLinesPage)}?shipmentId={item.Id}");
    }

    public async Task InitializeAsync()
    {
        await LoadAsync();
    }

    public async Task LoadAsync()
    {
        if (IsLoading)
            return;

        try
        {
            IsLoading = true;
            HasError = false;
            ErrorMessage = string.Empty;

            Items.Clear();

            var result = await _shipmentService
                .GetShipmentHeadersAsync(
                    Guid.Parse(CompanyId),
                    PageNumber,
                    PageSize);

            if (result?.Data != null)
            {
                _allItems = result.Data.ToList();

                foreach (var item in _allItems)
                    Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;

            OnPropertyChanged(nameof(HasData));
            OnPropertyChanged(nameof(IsEmpty));
        }
    }
}
