namespace Handheld.Models;

public class ServiceRequestDto
{
    public int RequestID { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int UserID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public int ServiceID { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? EstimatedTimeText { get; set; }
    public string? Category { get; set; }
    public bool PermiteAdjunto { get; set; }

    public string? ImageUrl { get; set; }
    public string? PaymentStatus { get; set; }

    public DateTime? ValidatedAt { get; set; }
    public int? ValidatedBy { get; set; }
    public string? AttachmentUrl { get; set; }
}
