namespace Handheld.Components.Bars;

public partial class Bar : ContentView
{
    public Bar()
    {
        InitializeComponent();
        Icon = "←"; // siempre flecha
    }

    // TITLE
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(Bar),
            string.Empty);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    // ICONO (ya lo estás usando en XAML)
    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(
            nameof(Icon),
            typeof(string),
            typeof(Bar),
            "←");

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    // CLICK IZQUIERDA = BACK
    private async void OnLeftClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}