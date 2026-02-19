

using System.Net.Http.Json;
using Handheld.Models;

namespace Handheld.Services;

public class MovementsService
{
    private readonly HttpClient _http;

    public MovementsService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<MovementsPageDto>> GetMovementsAsync()
    {
        return await _http.GetFromJsonAsync<List<MovementsPageDto>>(
            "api/movements"
        ) ?? new();
    }
}
