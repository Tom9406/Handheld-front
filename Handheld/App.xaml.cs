using Handheld.Services;

namespace Handheld;

public partial class App : Application
{
    public App(AppShell shell, StorageService storage)
    {
        InitializeComponent();

        MainPage = shell;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var token = await storage.GetToken();
            await shell.GoToAsync(string.IsNullOrWhiteSpace(token) ? "//login" : "//home");
        });
    }
}
