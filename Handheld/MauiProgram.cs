using Handheld.Services;
using Handheld.ViewModels;
using Handheld.Views;
using Microsoft.Extensions.Logging;

namespace Handheld;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // =========================
        // HTTP CLIENT
        // =========================
        builder.Services.AddHttpClient("ApiClient")
#if ANDROID
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
    })
#endif
    .ConfigureHttpClient(client =>
    {
        client.Timeout = TimeSpan.FromSeconds(60);
#if ANDROID
        var isEmulator = DeviceInfo.Current.DeviceType == DeviceType.Virtual;
        client.BaseAddress = new Uri(isEmulator ? "https://10.0.2.2:7177/" : "https://192.168.100.61:7177/");
#else
        client.BaseAddress = new Uri("https://localhost:7177/");
#endif
    });

        // =========================
        // SERVICES
        // =========================
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<StorageService>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<ChangePasswordViewModel>();
        builder.Services.AddTransient<ChangePasswordPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<AboutPage>();
        builder.Services.AddSingleton<ServiceRequestsService>();
        builder.Services.AddTransient<MyRequestsViewModel>();
        builder.Services.AddTransient<MyRequestsPage>();
        builder.Services.AddTransient<MyRequestDetails>();
        builder.Services.AddSingleton<ServicesService>();
        builder.Services.AddTransient<ServicesViewModel>();
        builder.Services.AddTransient<ServicesPage>();
        builder.Services.AddTransient<ServiceRequestPage>();
        return builder.Build();
    }
}
