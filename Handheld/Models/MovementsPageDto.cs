using System.Text.Json.Serialization;

namespace Handheld.Models;

public class MovementsPageDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("itemId")]
    public string ItemId { get; set; } = null!;

    [JsonPropertyName("itemNo")]
    public string ItemNo { get; set; } = null!;

    [JsonPropertyName("itemDescription")]
    public string ItemDescription { get; set; } = null!;

    [JsonPropertyName("binId")]
    public string BinId { get; set; } = null!;

    [JsonPropertyName("binCode")]
    public string BinCode { get; set; } = null!;

    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }

    [JsonPropertyName("movementType")]
    public string MovementType { get; set; } = null!;

    [JsonPropertyName("referenceNo")]
    public string ReferenceNo { get; set; } = null!;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
}
