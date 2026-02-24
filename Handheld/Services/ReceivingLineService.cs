using Handheld.Models;
using System.Net.Http.Json;

namespace Handheld.Services;

public class ReceivingLineService
{
    private readonly HttpClient _http;

    public ReceivingLineService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ReceivingLineDto>> GetByHeaderAsync(Guid headerId)
    {
        if (headerId == Guid.Empty)
            throw new ArgumentException("HeaderId es obligatorio.");

        var endpoint = $"api/receivinglines/by-header/{headerId}";

        var response = await _http.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"API Error: {error}");
        }

        var result = await response.Content
            .ReadFromJsonAsync<PagedResponse<ReceivingLineDto>>();

        return result?.Data ?? new List<ReceivingLineDto>();
    }
}