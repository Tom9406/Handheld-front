using Handheld.Views;

namespace Handheld;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("requestdetail", typeof(MyRequestDetails));
        Routing.RegisterRoute("mynewrequest", typeof(ServiceRequestPage));
        Routing.RegisterRoute("register", typeof(RegisterPage));
        Routing.RegisterRoute("change-password", typeof(ChangePasswordPage));
        Routing.RegisterRoute("about", typeof(AboutPage));
    }
}
