using Handheld.Models;
using Handheld.Services;
using Handheld.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Handheld.ViewModels
{
    public class ItemInquiryViewModel : BaseViewModel
    {
        private readonly ItemService _itemService;

        private const string CompanyId = "FC73E7BF-C62D-48FF-AC17-18244D67DFE4";

        public ObservableRangeCollection<ItemInquiryDto> Items { get; } = new();

        private int _pageNumber = 1;
        private const int PageSize = 20;
        private bool _hasMoreData = true;

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public bool HasData => Items.Count > 0;

        public override bool IsEmpty => !IsLoading && !HasError && !HasData;

        public ICommand SearchCommand { get; }
        public ICommand LoadMoreCommand { get; }

        public ItemInquiryViewModel(ItemService itemService)
        {
            _itemService = itemService;

            SearchCommand = new Command(async () => await SearchAsync());
            LoadMoreCommand = new Command(async () => await LoadMoreAsync());

            Items.CollectionChanged += (_, __) =>
            {
                OnPropertyChanged(nameof(HasData));
                OnPropertyChanged(nameof(IsEmpty));
            };
        }

        public async Task InitializeAsync()
        {
            await SearchAsync();
        }

        public async Task SearchAsync()
        {
            if (IsLoading)
                return;

            try
            {
                IsLoading = true;
                HasError = false;
                ErrorMessage = string.Empty;

                _pageNumber = 1;
                _hasMoreData = true;

                Items.Clear();

                await LoadPageAsync();
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task LoadMoreAsync()
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

                var result = await _itemService.SearchItemsAsync(
                    companyId: CompanyId,
                    itemNo: string.IsNullOrWhiteSpace(SearchText) ? null : SearchText,
                    binCode: null,
                    pageNumber: _pageNumber,
                    pageSize: PageSize);

                if (result?.Data != null && result.Data.Count > 0)
                {
                    Items.AddRange(result.Data);

                    _pageNumber++;
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
}