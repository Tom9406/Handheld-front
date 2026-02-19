using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace Handheld.Services;

public class ShipmentService
{
    private readonly HttpClient _httpClient;

    public ShipmentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedResponse<ShipmentHeaderDto>> GetShipmentHeadersAsync(
        Guid companyId,
        int pageNumber = 1,
        int pageSize = 20,
        string? status = null,
        string? shipmentNo = null,
        string sortBy = "CreatedAt",
        bool sortDesc = true)
    {
        var query = new StringBuilder();

        query.Append($"?pageNumber={pageNumber}");
        query.Append($"&pageSize={pageSize}");
        query.Append($"&companyId={companyId}");
        query.Append($"&sortBy={sortBy}");
        query.Append($"&sortDesc={sortDesc}");

        if (!string.IsNullOrWhiteSpace(status))
            query.Append($"&status={Uri.EscapeDataString(status)}");

        if (!string.IsNullOrWhiteSpace(shipmentNo))
            query.Append($"&shipmentNo={Uri.EscapeDataString(shipmentNo)}");

        var response = await _httpClient.GetAsync($"/api/shipmentheaders{query}");

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Error retrieving shipment headers: {response.StatusCode}");

        var result = await response.Content
            .ReadFromJsonAsync<PagedResponse<ShipmentHeaderDto>>();

        return result ?? new PagedResponse<ShipmentHeaderDto>();
    }
}
