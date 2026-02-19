using Handheld.Models;
using Handheld.Services;
using Handheld.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Handheld.ViewModels;

public class ReceivingHeadersViewModel : BaseViewModel
{
    private readonly ReceivingService _receivingService;

    // 🔹 Temporalmente fijo (igual que ItemInquiry)
    private const string CompanyId = "FC73E7BF-C62D-48FF-AC17-18244D67DFE4";

    private const int PageNumber = 1;
    private const int PageSize = 20;

    private List<ReceivingHeaderDto> _allItems = new();


    public ObservableCollection<ReceivingHeaderDto> Items { get; } = new();

    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public bool HasData => Items.Count > 0;

    public override bool IsEmpty => !IsLoading && !HasError && !HasData;

    public ICommand LoadCommand { get; }
    public ICommand SearchCommand { get; }

    public ReceivingHeadersViewModel(ReceivingService receivingService)
    {
        _receivingService = receivingService;

        LoadCommand = new Command(async () => await LoadAsync());
        SearchCommand = new Command(async () => await SearchAsync());

        Items.CollectionChanged += (_, __) =>
        {
            OnPropertyChanged(nameof(HasData));
            OnPropertyChanged(nameof(IsEmpty));
        };
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

            var result = await _receivingService
                .GetReceivingHeadersAsync(
                    CompanyId,
                    PageNumber,
                    PageSize);

            if (result?.Data != null)
            {
                _allItems = result.Data.ToList();

                Items.Clear();
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

    //  Filtro local por ReceiptNo
    public async Task SearchAsync()
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
                (x.ReceiptNo?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.ExternalDocumentNo?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.VendorCode?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.VendorName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.Status?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                //(x.CompanyName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.CreatedBy?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
            )
            .ToList();

        Items.Clear();
        foreach (var item in filtered)
            Items.Add(item);
    }

}
