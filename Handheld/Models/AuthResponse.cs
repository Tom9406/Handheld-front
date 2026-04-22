namespace Handheld.Models;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public bool MustChangePassword { get; set; }
}
