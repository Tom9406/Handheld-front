using Handheld.Models;

namespace Handheld.Services;

public class AuthService
{
    private readonly ApiService _apiService;
    private readonly StorageService _storage;

    public AuthService(ApiService apiService, StorageService storage)
    {
        _apiService = apiService;
        _storage = storage;
    }

    public async Task<AuthResponse?> Login(string username, string password, bool rememberSession)
    {
        var response = await _apiService.PostAsync<AuthResponse>("api/auth/login", new
        {
            username,
            password
        });

        if (response == null || string.IsNullOrWhiteSpace(response.Token))
            return null;

        await _storage.SaveToken(response.Token, rememberSession);
        _storage.SaveLastUsername(username);
        return response;
    }

    public async Task<AuthResponse?> Register(RegisterRequest request)
    {
        var response = await _apiService.PostAsync<AuthResponse>("api/auth/register", request);

        if (response == null || string.IsNullOrWhiteSpace(response.Token))
            return null;

        await _storage.SaveToken(response.Token, true);
        return response;
    }

    public async Task ChangePassword(string currentPassword, string newPassword)
    {
        await _apiService.PostAsync<object>("api/auth/change-password", new ChangePasswordRequest
        {
            CurrentPassword = currentPassword,
            NewPassword = newPassword
        });
    }

    public void Logout()
    {
        _storage.RemoveToken();
    }
}
