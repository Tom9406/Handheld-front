using Handheld.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Handheld;

public partial class AppShell : Shell
{
    private readonly IServiceProvider _services;

    public AppShell(IServiceProvider services)
    {
        InitializeComponent();
        _services = services;

        Items.Add(CreateShellContent<LoginPage>("login"));
        Items.Add(CreateShellContent<HomePage>("home"));

        Routing.RegisterRoute("myrequests", new ServiceProviderRouteFactory<MyRequestsPage>(_services));
        Routing.RegisterRoute("services", new ServiceProviderRouteFactory<ServicesPage>(_services));
        Routing.RegisterRoute("requestdetail", new ServiceProviderRouteFactory<MyRequestDetails>(_services));
        Routing.RegisterRoute("mynewrequest", new ServiceProviderRouteFactory<ServiceRequestPage>(_services));
        Routing.RegisterRoute("register", new ServiceProviderRouteFactory<RegisterPage>(_services));
        Routing.RegisterRoute("change-password", new ServiceProviderRouteFactory<ChangePasswordPage>(_services));
        Routing.RegisterRoute("about", new ServiceProviderRouteFactory<AboutPage>(_services));
    }

    private ShellContent CreateShellContent<TPage>(string route)
        where TPage : Page
    {
        return new ShellContent
        {
            Route = route,
            ContentTemplate = new DataTemplate(() => _services.GetRequiredService<TPage>())
        };
    }

    private sealed class ServiceProviderRouteFactory<TPage> : RouteFactory
        where TPage : Element
    {
        private readonly IServiceProvider _services;

        public ServiceProviderRouteFactory(IServiceProvider services)
        {
            _services = services;
        }

        public override Element GetOrCreate()
        {
            return _services.GetRequiredService<TPage>();
        }

        public override Element GetOrCreate(IServiceProvider services)
        {
            return services.GetRequiredService<TPage>();
        }
    }
}
