using System.Windows.Input;
using Handheld.Services;

namespace Handheld.ViewModels;

public class LoginViewModel : ViewModels.Base.BaseViewModel
{
    private readonly AuthService _authService;
    private readonly StorageService _storage;

    private string _username = string.Empty;
    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    private bool _rememberSession = true;
    public bool RememberSession
    {
        get => _rememberSession;
        set
        {
            if (SetProperty(ref _rememberSession, value))
                OnPropertyChanged(nameof(SessionHint));
        }
    }

    public string SessionHint =>
        RememberSession
            ? "Mantendremos tu sesion lista para la proxima vez."
            : "Al cerrar la app, volveras a ingresar tus datos.";

    public ICommand LoginCommand { get; }
    public ICommand GoToRegisterCommand { get; }

    public LoginViewModel(AuthService authService, StorageService storage)
    {
        _authService = authService;
        _storage = storage;
        Username = _storage.GetLastUsername();
        RememberSession = _storage.GetRememberSession();
        LoginCommand = new Command(async () => await Login());
        GoToRegisterCommand = new Command(async () => await Shell.Current.GoToAsync("register"));
    }

    private async Task Login()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            await Application.Current!.MainPage!.DisplayAlert("Campos requeridos", "Ingresa tu usuario y contrasena.", "OK");
            return;
        }

        try
        {
            IsBusy = true;

            var response = await _authService.Login(Username.Trim(), Password, RememberSession);

            if (response == null)
            {
                await Application.Current!.MainPage!.DisplayAlert("Acceso denegado", "Usuario o contrasena incorrectos.", "OK");
                return;
            }

            if (response.MustChangePassword)
            {
                await Shell.Current.GoToAsync("change-password?forced=true");
                return;
            }

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
}
