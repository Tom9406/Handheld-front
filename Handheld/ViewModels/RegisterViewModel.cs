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

    public ICommand RegisterCommand { get; }
    public ICommand BackToLoginCommand { get; }

    public RegisterViewModel(AuthService authService)
    {
        _authService = authService;
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

        try
        {
            IsBusy = true;

            var response = await _authService.Register(new RegisterRequest
            {
                Username = Username.Trim(),
                FirstName = FirstName.Trim(),
                LastName = LastName.Trim(),
                Phone = Phone.Trim(),
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
            await Application.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
