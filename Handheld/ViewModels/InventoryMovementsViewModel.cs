using Handheld.Models;
using Handheld.Services;
using Handheld.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Handheld.ViewModels;

public class InventoryMovementsViewModel : BaseViewModel
{
    private readonly InventoryMovementService _movementService;

    private const string CompanyId = "FC73E7BF-C62D-48FF-AC17-18244D67DFE4";

    public ObservableRangeCollection<MovementsPageDto> Movements { get; } = new();

    private int _pageNumber = 1;
    private const int PageSize = 20;
    private bool _hasMoreData = true;

    #region Search

    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    #endregion

    #region UI State

    public bool HasData => Movements.Count > 0;

    public override bool IsEmpty =>
        !IsLoading && !HasError && !HasData;

    #endregion

    #region Commands

    public ICommand SearchCommand { get; }
    public ICommand LoadMoreCommand { get; }

    #endregion

    public InventoryMovementsViewModel(InventoryMovementService movementService)
    {
        _movementService = movementService;

        SearchCommand = new Command(async () => await SearchAsync());
        LoadMoreCommand = new Command(async () => await LoadMoreAsync());

        Movements.CollectionChanged += (_, __) =>
        {
            OnPropertyChanged(nameof(HasData));
            OnPropertyChanged(nameof(IsEmpty));
        };
    }

    public async Task InitializeAsync()
    {
        await SearchAsync();
    }

    private async Task SearchAsync()
    {
        if (IsLoading)
            return;

        _pageNumber = 1;
        _hasMoreData = true;

        Movements.Clear();

        await LoadPageAsync();
    }

    private async Task LoadMoreAsync()
    {
        if (IsLoading || !_hasMoreData)
            return;

        await LoadPageAsync();
    }

    private async Task LoadPageAsync()
    {
        try
        {
            IsLoading = true;

            var result = await _movementService.SearchMovementsAsync(
                companyId: CompanyId,
                itemNo: string.IsNullOrWhiteSpace(SearchText) ? null : SearchText,
                binCode: null,
                movementType: null,
                referenceNo: null,
                pageNumber: _pageNumber,
                pageSize: PageSize);

            if (result?.Data != null && result.Data.Count > 0)
            {
                Movements.AddRange(result.Data);

                _pageNumber++;

                if (result.Data.Count < PageSize)
                    _hasMoreData = false;
            }
            else
            {
                _hasMoreData = false;
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}