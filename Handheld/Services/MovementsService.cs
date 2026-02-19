using Handheld.Models;
using System.Net.Http.Json;

namespace Handheld.Services;

public class InventoryMovementService
{
    private readonly HttpClient _http;

    public InventoryMovementService(HttpClient http)
    {
        _http = http;
    }

    public async Task<PagedResponse<MovementsPageDto>> SearchMovementsAsync(
        string companyId,
        string? itemNo,
        string? binCode,
        string? movementType,
        string? referenceNo,
        int pageNumber,
        int pageSize)
    {
        if (string.IsNullOrWhiteSpace(companyId))
            throw new ArgumentException("CompanyId es obligatorio.");

        var queryParams = new List<string>
        {
            $"companyId={Uri.EscapeDataString(companyId)}",
            $"pageNumber={pageNumber}",
            $"pageSize={pageSize}"
        };

        if (!string.IsNullOrWhiteSpace(itemNo))
            queryParams.Add($"itemNo={Uri.EscapeDataString(itemNo)}");

        if (!string.IsNullOrWhiteSpace(binCode))
            queryParams.Add($"binCode={Uri.EscapeDataString(binCode)}");

        if (!string.IsNullOrWhiteSpace(movementType))
            queryParams.Add($"movementType={Uri.EscapeDataString(movementType)}");

        if (!string.IsNullOrWhiteSpace(referenceNo))
            queryParams.Add($"referenceNo={Uri.EscapeDataString(referenceNo)}");

        var endpoint = $"api/movements?{string.Join("&", queryParams)}";


        var result = await _http.GetFromJsonAsync<PagedResponse<MovementsPageDto>>(endpoint);

        return result ?? new PagedResponse<MovementsPageDto>();
    }
}
