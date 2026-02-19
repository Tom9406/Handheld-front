using AndroidX.Lifecycle;
using Handheld.ViewModels;
using System.Windows.Input;

namespace Handheld.Views;

public partial class MainPage : ContentPage
{
    

    public MainPage()
    {
        InitializeComponent();
        // Ahora el BindingContext tiene acceso a todas las propiedades internas
        BindingContext = new MainViewModel(); 
    }

    private async Task GoToPage(string route)
    {
        await Shell.Current.GoToAsync(route);
    }



}
