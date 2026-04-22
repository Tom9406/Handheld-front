using System.Windows.Input;
using Handheld.Services;

namespace Handheld.ViewModels;

public class ChangePasswordViewModel : ViewModels.Base.BaseViewModel
{
    private readonly AuthService _authService;

    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public bool IsForced { get; set; }

    public ICommand ChangePasswordCommand { get; }
    public ICommand CancelCommand { get; }

    public ChangePasswordViewModel(AuthService authService)
    {
        _authService = authService;
        ChangePasswordCommand = new Command(async () => await ChangePassword());
        CancelCommand = new Command(async () => await Cancel());
    }

    private async Task ChangePassword()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(CurrentPassword) ||
            string.IsNullOrWhiteSpace(NewPassword) ||
            string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            await Application.Current!.MainPage!.DisplayAlert("Campos requeridos", "Completa todos los campos.", "OK");
            return;
        }

        if (NewPassword != ConfirmPassword)
        {
            await Application.Current!.MainPage!.DisplayAlert("Validacion", "La nueva contrasena y su confirmacion no coinciden.", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            await _authService.ChangePassword(CurrentPassword, NewPassword);
            await Application.Current!.MainPage!.DisplayAlert("Listo", "Tu contrasena fue actualizada.", "OK");
            await Shell.Current.GoToAsync("//home");
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

    private async Task Cancel()
    {
        if (IsForced)
        {
            _authService.Logout();
            await Shell.Current.GoToAsync("//login");
            return;
        }

        await Shell.Current.GoToAsync("..");
    }
}
