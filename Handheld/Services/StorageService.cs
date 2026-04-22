namespace Handheld.Services;

public class StorageService
{
    public async Task SaveToken(string token)
    {
        await SecureStorage.SetAsync("jwt_token", token);
    }

    public async Task<string?> GetToken()
    {
        return await SecureStorage.GetAsync("jwt_token");
    }

    public void RemoveToken()
    {
        SecureStorage.Remove("jwt_token");
    }
}
