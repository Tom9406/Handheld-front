using Handheld.ViewModels;

namespace Handheld.Views;

public partial class RegisterCompanyPage : ContentPage
{
    public RegisterCompanyPage(CompanyViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
