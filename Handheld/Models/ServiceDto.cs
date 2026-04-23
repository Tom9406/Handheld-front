using System.Text.Json.Serialization;

namespace Handheld.Models;

public class ServiceDto
{
    public int ServiceID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Category { get; set; }
    public bool IsActive { get; set; } = true;

    [JsonPropertyName("permiteAdjunto")]
    public bool PermiteAdjunto { get; set; }

    [JsonPropertyName("estimatedTimeText")]
    public string? EstimatedTimeText { get; set; }
}
