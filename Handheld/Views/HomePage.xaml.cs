namespace Handheld.Views;

public partial class HomePage : ContentPage
{
	private readonly Services.AuthService _authService;

	public HomePage(Services.AuthService authService)
	{
		InitializeComponent();
		_authService = authService;
	}

    private async void OnAboutClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("about");
    }

    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("change-password");
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        _authService.Logout();
        await Shell.Current.GoToAsync("//login");
    }
}
