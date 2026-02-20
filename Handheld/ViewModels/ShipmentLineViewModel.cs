using Handheld.Models;
using Handheld.Services;
using Handheld.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Handheld.ViewModels
{
    public class ShipmentLineViewModel : BaseViewModel
    {
        private readonly ShipmentLineService _shipmentLineService;

        private const string CompanyId = "FC73E7BF-C62D-48FF-AC17-18244D67DFE4";

        private string _shipmentId;
        public string ShipmentId
        {
            get => _shipmentId;
            set => SetProperty(ref _shipmentId, value);
        }

        // 🔹 Lista completa
        private List<ShipmentLineDto> _allLines = new();

        // 🔹 Lista visible
        public ObservableCollection<ShipmentLineDto> Lines { get; } = new();

        // 🔹 FILTRO AUTOMÁTICO
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    FilterLines(); // 🔥 Se ejecuta al escribir o escanear
                }
            }
        }

        private string _statusFilter;
        public string StatusFilter
        {
            get => _statusFilter;
            set => SetProperty(ref _statusFilter, value);
        }

        public bool HasData => Lines.Count > 0;
        public override bool IsEmpty => !IsLoading && !HasError && !HasData;

        public ICommand LoadCommand { get; }

        public ShipmentLineViewModel(ShipmentLineService shipmentLineService)
        {
            _shipmentLineService = shipmentLineService;

            LoadCommand = new Command(async () => await LoadAsync());

            Lines.CollectionChanged += (_, __) =>
            {
                OnPropertyChanged(nameof(HasData));
                OnPropertyChanged(nameof(IsEmpty));
            };
        }

        public async Task InitializeAsync(string shipmentId)
        {
            ShipmentId = shipmentId;
            await LoadAsync();
        }

        public async Task LoadAsync()
        {
            if (IsLoading || string.IsNullOrWhiteSpace(ShipmentId))
                return;

            try
            {
                IsLoading = true;
                HasError = false;
                ErrorMessage = string.Empty;

                Lines.Clear();
                _allLines.Clear();

                var result = await _shipmentLineService.GetShipmentLinesAsync(
                    companyId: CompanyId,
                    shipmentId: ShipmentId,
                    status: string.IsNullOrWhiteSpace(StatusFilter) ? null : StatusFilter,
                    pageNumber: 1,
                    pageSize: 20);

                if (result?.Data != null)
                {
                    _allLines = result.Data.ToList();

                    foreach (var line in _allLines)
                        Lines.Add(line);
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

        private void FilterLines()
        {
            if (_allLines == null)
                return;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Lines.Clear();
                foreach (var line in _allLines)
                    Lines.Add(line);
                return;
            }

            var search = SearchText.Trim();

            var filtered = _allLines
                .Where(x =>
                    (x.ItemNo?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.ItemDescription?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.UnitOfMeasure?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.BinCode?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.LineStatus?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    x.LineNo.ToString().Contains(search) ||
                    x.OrderedQty.ToString().Contains(search) ||
                    x.PickedQty.ToString().Contains(search) ||
                    x.ShippedQty.ToString().Contains(search) ||
                    x.RemainingQty.ToString().Contains(search)
                )
                .ToList();

            Lines.Clear();
            foreach (var line in filtered)
                Lines.Add(line);
        }
    }
}