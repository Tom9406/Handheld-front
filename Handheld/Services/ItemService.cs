using Handheld.Models;
using System.Net.Http.Json;

namespace Handheld.Services;

public class ItemService
{
    private readonly HttpClient _http;

    public ItemService(HttpClient http)
    {
        _http = http;
    }

    public async Task<PagedResponse<ItemInquiryDto>> SearchItemsAsync(
        string companyId,
        string? itemNo,
        string? binCode,
        int pageNumber,
        int pageSize)
    {
        if (string.IsNullOrWhiteSpace(companyId))
            throw new ArgumentException("CompanyId es obligatorio.");

        var queryParams = new List<string>
        {
            $"companyId={Uri.EscapeDataString(companyId)}",
            $"PageNumber={pageNumber}",
            $"PageSize={pageSize}"
        };

        if (!string.IsNullOrWhiteSpace(itemNo))
            queryParams.Add($"itemNo={Uri.EscapeDataString(itemNo)}");

        if (!string.IsNullOrWhiteSpace(binCode))
            queryParams.Add($"binCode={Uri.EscapeDataString(binCode)}");

        var endpoint = $"api/stock/enriched?{string.Join("&", queryParams)}";

        var response = await _http.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"API Error: {error}");
        }

        var result = await response.Content
            .ReadFromJsonAsync<PagedResponse<ItemInquiryDto>>();

        return result ?? new PagedResponse<ItemInquiryDto>();
    }
}
