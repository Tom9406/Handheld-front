using System.Text.Json.Serialization;

namespace Handheld.Models;

public class MovementsPageDto
{
    [JsonPropertyName("id")]
    public string MovId { get; set; } = null!;

    [JsonPropertyName("itemNo")]
    public string ItemNo { get; set; } = null!;

    [JsonPropertyName("binCode")]
    public string BinCode { get; set; } = null!;

    [JsonPropertyName("qty")]
    public decimal Quantity { get; set; }

    [JsonPropertyName("movementType")]
    public string MovementType { get; set; } = null!;

    [JsonPropertyName("referenceNo")]
    public string ReferenceNo { get; set; } = null!;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
}
