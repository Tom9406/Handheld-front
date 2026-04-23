using System.Windows.Input;
using Handheld.Models;
using Handheld.Services;

namespace Handheld.ViewModels;

public class RegisterViewModel : ViewModels.Base.BaseViewModel
{
    private readonly AuthService _authService;

    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<CountryPhoneOption> CountryOptions { get; } = new()
    {
        new CountryPhoneOption("Paraguay", "+595", "PY"),
        new CountryPhoneOption("Brasil", "+55", "BR"),
        new CountryPhoneOption("Argentina", "+54", "AR"),
        new CountryPhoneOption("Estados Unidos", "+1", "US")
    };

    private CountryPhoneOption? _selectedCountry;
    public CountryPhoneOption? SelectedCountry
    {
        get => _selectedCountry;
        set => SetProperty(ref _selectedCountry, value);
    }

    public ICommand RegisterCommand { get; }
    public ICommand BackToLoginCommand { get; }

    public RegisterViewModel(AuthService authService)
    {
        _authService = authService;
        SelectedCountry = CountryOptions[0];
        RegisterCommand = new Command(async () => await Register());
        BackToLoginCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
    }

    private async Task Register()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(FirstName) ||
            string.IsNullOrWhiteSpace(LastName) ||
            string.IsNullOrWhiteSpace(Phone) ||
            string.IsNullOrWhiteSpace(Email))
        {
            await Application.Current!.MainPage!.DisplayAlert("Campos requeridos", "Completa todos los datos para registrarte.", "OK");
            return;
        }

        if (!IsValidEmail(Email.Trim()))
        {
            await Application.Current!.MainPage!.DisplayAlert("Correo invalido", "Ingresa un correo valido.", "OK");
            return;
        }

        var normalizedPhone = NormalizePhone(Phone);

        if (normalizedPhone.Length < 6 || normalizedPhone.Length > 15)
        {
            await Application.Current!.MainPage!.DisplayAlert("Telefono invalido", "Ingresa solo numeros y verifica la longitud del telefono.", "OK");
            return;
        }

        try
        {
            IsBusy = true;

            var response = await _authService.Register(new RegisterRequest
            {
                Username = Username.Trim(),
                FirstName = FirstName.Trim(),
                LastName = LastName.Trim(),
                Phone = $"{SelectedCountry?.DialCode}{normalizedPhone}",
                Email = Email.Trim()
            });

            if (response == null)
            {
                await Application.Current!.MainPage!.DisplayAlert("Registro fallido", "No se pudo crear tu cuenta.", "OK");
                return;
            }

            await Application.Current!.MainPage!.DisplayAlert("Cuenta creada", "Tu cuenta fue creada con password temporal 123456. Debes cambiarla ahora.", "OK");
            await Shell.Current.GoToAsync("change-password?forced=true");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("No se pudo crear la cuenta", GetFriendlyError(ex), "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var address = new System.Net.Mail.MailAddress(email);
            return address.Address == email && email.Contains('.');
        }
        catch
        {
            return false;
        }
    }

    private static string NormalizePhone(string phone)
    {
        return new string(phone.Where(char.IsDigit).ToArray());
    }

    private static string GetFriendlyError(Exception ex)
    {
        if (ex.Message.Contains("Username ya registrado", StringComparison.OrdinalIgnoreCase))
            return "El usuario ya esta registrado. Elige otro nombre de usuario.";

        if (ex.Message.Contains("Email ya registrado", StringComparison.OrdinalIgnoreCase))
            return "El correo ya esta registrado. Ingresa otro correo.";

        if (ex.Message.Contains("Invalid credentials", StringComparison.OrdinalIgnoreCase))
            return "Usuario o contrasena incorrectos.";

        return string.IsNullOrWhiteSpace(ex.Message)
            ? "Verifica los datos e intenta nuevamente."
            : ex.Message;
    }
}

public record CountryPhoneOption(string Name, string DialCode, string Code)
{
    public string DisplayName => $"{Name} ({DialCode})";
}
