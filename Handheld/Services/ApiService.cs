using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Handheld.Services;

public class ApiService
{
    private readonly HttpClient _client;
    private readonly StorageService _storage;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ApiService(IHttpClientFactory factory, StorageService storage)
    {
        _client = factory.CreateClient("ApiClient");
        _storage = storage;
    }

    private async Task AddAuthorizationHeader()
    {
        var token = await _storage.GetToken();

        if (string.IsNullOrWhiteSpace(token))
        {
            _client.DefaultRequestHeaders.Authorization = null;
            return;
        }

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    private async Task<T?> ReadResponse<T>(HttpResponseMessage response)
    {
        var payload = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            if (string.IsNullOrWhiteSpace(payload))
                throw new InvalidOperationException($"Error HTTP {(int)response.StatusCode}");

            throw new InvalidOperationException(ExtractErrorMessage(payload));
        }

        if (string.IsNullOrWhiteSpace(payload))
            return default;

        return JsonSerializer.Deserialize<T>(payload, _jsonOptions);
    }

    private static string ExtractErrorMessage(string payload)
    {
        try
        {
            using var doc = JsonDocument.Parse(payload);
            var root = doc.RootElement;

            if (root.TryGetProperty("error", out var error))
            {
                if (error.ValueKind == JsonValueKind.Object &&
                    error.TryGetProperty("message", out var message))
                {
                    return message.GetString() ?? "No se pudo completar la operacion.";
                }

                if (error.ValueKind == JsonValueKind.String)
                    return error.GetString() ?? "No se pudo completar la operacion.";
            }

            if (root.TryGetProperty("message", out var rootMessage))
                return rootMessage.GetString() ?? "No se pudo completar la operacion.";
        }
        catch
        {
            // If the payload is not JSON, show the original text.
        }

        return payload;
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        await AddAuthorizationHeader();
        var response = await _client.GetAsync(url);
        return await ReadResponse<T>(response);
    }

    public async Task<T?> PostAsync<T>(string url, object data)
    {
        await AddAuthorizationHeader();

        var json = JsonSerializer.Serialize(data);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(url, content);

        return await ReadResponse<T>(response);
    }

    public async Task<T?> PutAsync<T>(string url, object data)
    {
        await AddAuthorizationHeader();

        var json = JsonSerializer.Serialize(data);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PutAsync(url, content);

        return await ReadResponse<T>(response);
    }

    public async Task<T?> PostMultipartAsync<T>(string url, string filePath, string fieldName)
    {
        await AddAuthorizationHeader();

        await using var stream = File.OpenRead(filePath);
        using var streamContent = new StreamContent(stream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(GetMimeType(filePath));

        using var content = new MultipartFormDataContent();
        content.Add(streamContent, fieldName, Path.GetFileName(filePath));

        var response = await _client.PostAsync(url, content);
        return await ReadResponse<T>(response);
    }

    private static string GetMimeType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();

        return extension switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".pdf" => "application/pdf",
            _ => "application/octet-stream"
        };
    }
}
