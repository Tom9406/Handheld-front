namespace Handheld.Models;

public class CreateServiceRequestRequest
{
    public int ServiceID { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}
