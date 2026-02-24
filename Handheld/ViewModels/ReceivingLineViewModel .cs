using Handheld.Models;
using Handheld.Services;
using Handheld.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Handheld.ViewModels
{
    public class ReceivingLineViewModel : BaseViewModel
    {
        private readonly ReceivingLineService _receivingLineService;

        // Igual que en Shipment, lo estás dejando fijo
        private static readonly Guid CompanyId =
            Guid.Parse("FC73E7BF-C62D-48FF-AC17-18244D67DFE4");

        private Guid _receivingHeaderId;
        public Guid ReceivingHeaderId
        {
            get => _receivingHeaderId;
            set => SetProperty(ref _receivingHeaderId, value);
        }

        // 🔹 Lista completa
        private List<ReceivingLineDto> _allLines = new();

        // 🔹 Lista visible
        public ObservableCollection<ReceivingLineDto> Lines { get; } = new();

        // 🔹 Búsqueda
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    FilterLines();
            }
        }

        public bool HasData => Lines.Count > 0;
        public override bool IsEmpty => !IsLoading && !HasError && !HasData;

        public ICommand LoadCommand { get; }

        public ReceivingLineViewModel(ReceivingLineService receivingLineService)
        {
            _receivingLineService = receivingLineService;

            LoadCommand = new Command(async () => await LoadAsync());

            Lines.CollectionChanged += (_, __) =>
            {
                OnPropertyChanged(nameof(HasData));
                OnPropertyChanged(nameof(IsEmpty));
            };
        }

        public async Task InitializeAsync(Guid headerId)
        {
            ReceivingHeaderId = headerId;
            await LoadAsync();
        }

        public async Task LoadAsync()
        {
            if (IsLoading || ReceivingHeaderId == Guid.Empty)
                return;

            try
            {
                IsLoading = true;
                HasError = false;
                ErrorMessage = string.Empty;

                Lines.Clear();
                _allLines.Clear();

                var result = await _receivingLineService
                    .GetByHeaderAsync(ReceivingHeaderId);

                if (result != null)
                {
                    _allLines = result.ToList();

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
                    x.UOM.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    x.QuantityExpected.ToString().Contains(search) ||
                    x.QuantityReceived.ToString().Contains(search) ||
                    x.ItemCode.ToString().Contains(search) ||
                    x.BinId.ToString().Contains(search)
                )
                .ToList();

            Lines.Clear();
            foreach (var line in filtered)
                Lines.Add(line);
        }
    }
}