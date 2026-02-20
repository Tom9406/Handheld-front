using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace Handheld.Models
{
    public class ShipmentLineDto : INotifyPropertyChanged
    {
        // Contexto
        public string CompanyId { get; set; }
        public string ShipmentId { get; set; }

        // Línea
        public string Id { get; set; }
        public int LineNo { get; set; }
        public string LineStatus { get; set; }

        // Item
        public string ItemId { get; set; }
        public string ItemNo { get; set; }
        public string ItemDescription { get; set; }
        public string UnitOfMeasure { get; set; }

        // Ubicación
        public string WarehouseId { get; set; }
        public string BinId { get; set; }
        public string BinCode { get; set; }

        // Cantidades
        public decimal OrderedQty { get; set; }
        public decimal PickedQty { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private decimal _shippedQty;
        public decimal ShippedQty
        {
            get => _shippedQty;
            set
            {
                var newValue = value;

                // 🔒 Limitar visualmente al máximo permitido
                if (newValue > OrderedQty)
                    newValue = OrderedQty;

                if (newValue < 0)
                    newValue = 0;

                if (_shippedQty != newValue)
                {
                    _shippedQty = newValue;
                    OnPropertyChanged();
                }
            }
        }
        public decimal? BaseUomQty { get; set; }
        public decimal RemainingQty { get; set; }

        // Tracking
        public string LotNo { get; set; }
        public string SerialNo { get; set; }
        public DateTime? ExpirationDate { get; set; }

        // Dimensiones
        public decimal? UnitWeight { get; set; }
        public decimal? UnitVolume { get; set; }

        // Estado
        public bool IsCompleted { get; set; }

        // Auditoría
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
