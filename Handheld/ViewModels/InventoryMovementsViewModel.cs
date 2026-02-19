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

    public ObservableCollection<MovementsPageDto> Movements { get; } = new();

    #region Search

    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    #endregion

    #region Pagination

    private int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        set => SetProperty(ref _currentPage, value);
    }

    private int _totalPages;
    public int TotalPages
    {
        get => _totalPages;
        set => SetProperty(ref _totalPages, value);
    }

    #endregion

    #region UI State

    public bool HasData => Movements.Count > 0;

    public override bool IsEmpty =>
        !IsLoading && !HasError && !HasData;

    #endregion

    #region Commands

    public ICommand SearchCommand { get; }
    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }

    #endregion

    public InventoryMovementsViewModel(InventoryMovementService movementService)
    {
        _movementService = movementService;

        SearchCommand = new Command(async () => await SearchAsync());
        NextPageCommand = new Command(async () => await NextPageAsync());
        PreviousPageCommand = new Command(async () => await PreviousPageAsync());

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
        CurrentPage = 1;
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        if (IsLoading)
            return;

        try
        {
            IsLoading = true;
            HasError = false;
            ErrorMessage = string.Empty;

            Movements.Clear();

            var result = await _movementService.SearchMovementsAsync(
                companyId: CompanyId,
                itemNo: string.IsNullOrWhiteSpace(SearchText) ? null : SearchText,
                binCode: null,
                movementType: null,
                referenceNo: null,
                pageNumber: CurrentPage,
                pageSize: 20);

            if (result?.Data != null)
            {
                foreach (var movement in result.Data)
                    Movements.Add(movement);

                TotalPages = result.TotalPages;
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

    private async Task NextPageAsync()
    {
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
            await LoadAsync();
        }
    }

    private async Task PreviousPageAsync()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            await LoadAsync();
        }
    }
}
