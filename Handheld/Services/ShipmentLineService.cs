using Handheld.Models;
using System.Net.Http.Json;

namespace Handheld.Services;

public class ShipmentLineService
{
    private readonly HttpClient _http;

    public ShipmentLineService(HttpClient http)
    {
        _http = http;
    }

    public async Task<PagedResponse<ShipmentLineDto>> GetShipmentLinesAsync(
        string companyId,
        string shipmentId,
        string? status,
        int pageNumber,
        int pageSize)
    {
        if (string.IsNullOrWhiteSpace(companyId))
            throw new ArgumentException("CompanyId es obligatorio.");

        if (string.IsNullOrWhiteSpace(shipmentId))
            throw new ArgumentException("ShipmentId es obligatorio.");

        var queryParams = new List<string>
        {
            $"companyId={Uri.EscapeDataString(companyId)}",
            $"shipmentId={Uri.EscapeDataString(shipmentId)}",
            $"pageNumber={pageNumber}",
            $"pageSize={pageSize}"
        };

        if (!string.IsNullOrWhiteSpace(status))
            queryParams.Add($"status={Uri.EscapeDataString(status)}");

        var endpoint = $"api/shipmentlines?{string.Join("&", queryParams)}";

        var response = await _http.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"API Error: {error}");
        }

        var result = await response.Content
            .ReadFromJsonAsync<PagedResponse<ShipmentLineDto>>();

        return result ?? new PagedResponse<ShipmentLineDto>();
    }
}
