using System.Net.Http.Json;
using Handheld.Models;

namespace Handheld.Services;

public class PickService
{
    private readonly HttpClient _http;

    public PickService(HttpClient http)
    {
        _http = http;
    }

    public async Task<PagedResponse<PickHeaderDto>> GetPicksAsync(
    int pageNumber,
    int pageSize)
    {
        return await _http.GetFromJsonAsync<PagedResponse<PickHeaderDto>>(
            $"api/pickheaders?pageNumber={pageNumber}&pageSize={pageSize}"
        ) ?? new PagedResponse<PickHeaderDto>();
    }

}
