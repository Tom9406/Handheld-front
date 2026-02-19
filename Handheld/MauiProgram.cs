using Handheld.Services;
using Handheld.ViewModels;
using Handheld.Views;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Handheld
{
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
            // API / SERVICES
            // =========================
            builder.Services.AddHttpClient<ItemService>()
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
#if ANDROID
                    client.BaseAddress = new Uri("https://10.0.2.2:7216/");
#else
                    client.BaseAddress = new Uri("https://localhost:7216/");
#endif
                });

            builder.Services.AddHttpClient<PickService>()
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
#if ANDROID
                    client.BaseAddress = new Uri("https://10.0.2.2:7216/");
#else
                    client.BaseAddress = new Uri("https://localhost:7216/");
#endif
                });

            builder.Services.AddHttpClient<MovementsService>()
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
#if ANDROID
                    client.BaseAddress = new Uri("https://10.0.2.2:7216/");
#else
                    client.BaseAddress = new Uri("https://localhost:7216/");
#endif
                });


            builder.Services.AddHttpClient<ReceivingService>()
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
#if ANDROID
        client.BaseAddress = new Uri("https://10.0.2.2:7216/");
#else
        client.BaseAddress = new Uri("https://localhost:7216/");
#endif
    });

            builder.Services.AddHttpClient<ShipmentService>()
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
#if ANDROID
        client.BaseAddress = new Uri("https://10.0.2.2:7216/");
#else
        client.BaseAddress = new Uri("https://localhost:7216/");
#endif
    });


            builder.Services.AddHttpClient<CompanyService>()
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
#if ANDROID
        client.BaseAddress = new Uri("https://10.0.2.2:7216/");
#else
        client.BaseAddress = new Uri("https://localhost:7216/");
#endif
    });

            builder.Services.AddHttpClient<ShipmentLineService>()
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
#if ANDROID
        client.BaseAddress = new Uri("https://10.0.2.2:7216/");
#else
        client.BaseAddress = new Uri("https://localhost:7216/");
#endif
    });




            // =========================
            // VIEWMODELS
            // =========================
            builder.Services.AddTransient<ItemInquiryViewModel>();
            builder.Services.AddTransient<PickHeadersViewModel>();
            builder.Services.AddTransient<ReceivingHeadersViewModel>();
            builder.Services.AddTransient<ShipmentHeadersViewModel>();
            builder.Services.AddTransient<ShipmentLineViewModel>();
            builder.Services.AddTransient<CompanyViewModel>();




            // =========================
            // PAGES
            // =========================
            builder.Services.AddTransient<ItemInquiryPage>();
            builder.Services.AddTransient<PickingPage>();
            builder.Services.AddTransient<MovementsPage>();
            builder.Services.AddTransient<ReceivingPage>();
            builder.Services.AddTransient<ShipmentHeadersPage>();
            builder.Services.AddTransient<ShipmentLinesPage>();
            builder.Services.AddTransient<RegisterCompanyPage>();



            return builder.Build();
        }
    }
}
