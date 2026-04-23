namespace Handheld.Services;

public class StorageService
{
    private string? _sessionToken;
    private const string TokenKey = "jwt_token";
    private const string RememberSessionKey = "remember_session";
    private const string LastUsernameKey = "last_username";

    public async Task SaveToken(string token, bool rememberSession)
    {
        _sessionToken = token;
        Preferences.Set(RememberSessionKey, rememberSession);

        if (rememberSession)
            await SecureStorage.SetAsync(TokenKey, token);
        else
            SecureStorage.Remove(TokenKey);
    }

    public async Task<string?> GetToken()
    {
        if (!string.IsNullOrWhiteSpace(_sessionToken))
            return _sessionToken;

        try
        {
            return Preferences.Get(RememberSessionKey, true)
                ? await SecureStorage.GetAsync(TokenKey)
                : null;
        }
        catch
        {
            SecureStorage.Remove(TokenKey);
            return null;
        }
    }

    public void SaveLastUsername(string username)
    {
        Preferences.Set(LastUsernameKey, username);
    }

    public string GetLastUsername()
    {
        return Preferences.Get(LastUsernameKey, string.Empty);
    }

    public bool GetRememberSession()
    {
        return Preferences.Get(RememberSessionKey, true);
    }

    public void RemoveToken()
    {
        _sessionToken = null;
        SecureStorage.Remove(TokenKey);
    }
}
