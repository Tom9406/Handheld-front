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

        // 🔹 Temporalmente fijo
        private const string CompanyId = "FC73E7BF-C62D-48FF-AC17-18244D67DFE4";

        public ObservableCollection<ItemInquiryDto> Items { get; } = new();

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public bool HasData => Items.Count > 0;

        public override bool IsEmpty => !IsLoading && !HasError && !HasData;

        public ICommand SearchCommand { get; }

        public ItemInquiryViewModel(ItemService itemService)
        {
            _itemService = itemService;

            SearchCommand = new Command(async () => await SearchAsync());

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

                Items.Clear();

                var result = await _itemService.SearchItemsAsync(
                    companyId: CompanyId,
                    itemNo: string.IsNullOrWhiteSpace(SearchText) ? null : SearchText,
                    binCode: null,
                    pageNumber: 1,
                    pageSize: 20);

                if (result?.Data != null)
                {
                    foreach (var item in result.Data)
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
}
