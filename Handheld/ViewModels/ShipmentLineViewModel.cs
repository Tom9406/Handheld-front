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

        // 🔹 Temporalmente fijo
        private const string CompanyId = "FC73E7BF-C62D-48FF-AC17-18244D67DFE4";

        // 🔹 Se asigna cuando se toca o escanea el header
        private string _shipmentId;
        public string ShipmentId
        {
            get => _shipmentId;
            set => SetProperty(ref _shipmentId, value);
        }

        public ObservableCollection<ShipmentLineDto> Lines { get; } = new();

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

                var result = await _shipmentLineService.GetShipmentLinesAsync(
                    companyId: CompanyId,
                    shipmentId: ShipmentId,
                    status: string.IsNullOrWhiteSpace(StatusFilter) ? null : StatusFilter,
                    pageNumber: 1,
                    pageSize: 20);

                // 🔎 DEBUG AQUÍ
                System.Diagnostics.Debug.WriteLine($"ShipmentId enviado: {ShipmentId}");
                System.Diagnostics.Debug.WriteLine($"Result null: {result == null}");
                System.Diagnostics.Debug.WriteLine($"Data null: {result?.Data == null}");
                System.Diagnostics.Debug.WriteLine($"Cantidad recibida: {result?.Data?.Count}");

                if (result?.Data != null)
                {
                    foreach (var line in result.Data)
                        Lines.Add(line);
                }

                // 🔎 DEBUG FINAL
                System.Diagnostics.Debug.WriteLine($"Lines.Count después de llenar: {Lines.Count}");
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;

                System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
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
