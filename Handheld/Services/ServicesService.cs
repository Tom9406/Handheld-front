using Handheld.Models;

namespace Handheld.Services;

public class ServicesService
{
    private readonly ApiService _api;

    public ServicesService(ApiService api)
    {
        _api = api;
    }

    public async Task<List<ServiceDto>> GetByCategory(string category)
    {
        var encodedCategory = Uri.EscapeDataString(category);

        return await _api.GetAsync<List<ServiceDto>>(
            $"api/services/by-category?category={encodedCategory}"
        ) ?? new List<ServiceDto>();
    }

    public async Task<ServiceDto?> GetServiceById(int id)
    {
        return await _api.GetAsync<ServiceDto>($"api/services/{id}");
    }
}
