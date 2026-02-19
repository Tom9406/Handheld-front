public class ShipmentHeaderDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string CompanyCode { get; set; } = string.Empty;

    public string ShipmentNo { get; set; } = string.Empty;
    public string? ExternalShipmentNo { get; set; }

    public string ShipmentType { get; set; } = string.Empty;
    public string ShipmentStatus { get; set; } = string.Empty;

    public string WarehouseCode { get; set; } = string.Empty;

    public string CustomerCode { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;

    public DateTime? PlannedShipDate { get; set; }
    public DateTime? ActualShipDate { get; set; }

    public int TotalLines { get; set; }
    public decimal TotalQty { get; set; }

    public bool IsClosed { get; set; }

    public DateTime CreatedAt { get; set; }
}
