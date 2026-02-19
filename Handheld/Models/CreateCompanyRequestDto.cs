namespace Handheld.Models;

public class CreateCompanyRequest
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;

    public string? CompanyType { get; set; }
    public string? LegalName { get; set; }
    public string? TaxId { get; set; }
    public string? Address1 { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}
