namespace Handheld.Models;

public class CompanyResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? CompanyType { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;
    public bool IsWmsEnabled { get; set; }
    public DateTime CreatedAt { get; set; }
}
