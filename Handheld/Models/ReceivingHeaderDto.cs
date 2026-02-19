namespace Handheld.Models;

public class ReceivingHeaderDto
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string ReceiptNo { get; set; } = string.Empty;

    public string? ExternalDocumentNo { get; set; }

    public string? VendorCode { get; set; }

    public string? VendorName { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime ReceiptDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? PostedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string? PostedBy { get; set; }
}
