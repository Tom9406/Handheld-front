using System.Text.Json.Serialization;

namespace Handheld.Models;

public class PickHeaderDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("pickNo")]
    public string PickNo { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("assignedUserName")]
    public string? AssignedUserName { get; set; }

    [JsonPropertyName("salesOrderNo")]
    public string? SalesOrderNo { get; set; }

    [JsonPropertyName("warehouseShipmentNo")]
    public string? WarehouseShipmentNo { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("completedAt")]
    public DateTime? CompletedAt { get; set; }
}
