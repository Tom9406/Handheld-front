using Handheld.Models;
using System.Net.Http.Json;

namespace Handheld.Services;

public class CompanyService
{
    private readonly HttpClient _httpClient;

    public CompanyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CompanyResponse> CreateCompanyAsync(CreateCompanyRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/companies", request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error creating company: {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<CompanyResponse>();

        if (result == null)
            throw new Exception("Company response was null.");

        return result;
    }
}
