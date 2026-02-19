using System.Net.Http.Json;
using Handheld.Models;

namespace Handheld.Services;

public class ReceivingService
{
    private readonly HttpClient _http;

    public ReceivingService(HttpClient http)
    {
        _http = http;
    }

    public async Task<PagedResponse<ReceivingHeaderDto>> GetReceivingHeadersAsync(
        string companyId,
        int pageNumber,
        int pageSize)
    {
        var url =
            $"api/ReceivingHeaders?" +
            $"companyId={companyId}" +
            $"&PageNumber={pageNumber}" +
            $"&PageSize={pageSize}";

        return await _http
            .GetFromJsonAsync<PagedResponse<ReceivingHeaderDto>>(url)
               ?? new PagedResponse<ReceivingHeaderDto>();
    }
}
