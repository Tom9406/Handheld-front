using Handheld.ViewModels;

namespace Handheld.Views;

[QueryProperty(nameof(Forced), "forced")]
public partial class ChangePasswordPage : ContentPage
{
    private readonly ChangePasswordViewModel _vm;

    public ChangePasswordPage(ChangePasswordViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    public string? Forced
    {
        set => _vm.IsForced = string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
    }
}
