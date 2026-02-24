namespace Handheld.Models;

public class ReceivingLineDto
{
    public Guid Id { get; set; }

    // Relaciones
    public Guid ReceivingHeaderId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid ItemId { get; set; }
    public Guid BinId { get; set; }
    public string ItemCode { get; set; } = null!;

    // Cantidades
    public decimal QuantityExpected { get; set; }
    public decimal QuantityReceived { get; set; }

    // Unidad de medida
    public string UOM { get; set; } = string.Empty;

    // Auditoría
    public DateTime CreatedAt { get; set; }
}