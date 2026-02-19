using System;

namespace Handheld.Models
{
    public class ShipmentLineDto
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
        public decimal ShippedQty { get; set; }
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
