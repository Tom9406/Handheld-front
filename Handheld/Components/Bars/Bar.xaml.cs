using System.Windows.Input;

namespace Handheld.Components.Bars;

public partial class Bar : ContentView
{
    public Bar()
    {
        InitializeComponent();
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

    // 👉 OPCIONAL (puedes eliminarlo si no lo usas)
    public static readonly BindableProperty MenuCommandProperty =
        BindableProperty.Create(
            nameof(MenuCommand),
            typeof(ICommand),
            typeof(Bar),
            default(ICommand));

    public ICommand MenuCommand
    {
        get => (ICommand)GetValue(MenuCommandProperty);
        set => SetValue(MenuCommandProperty, value);
    }

    //  CLAVE: abrir menú SIEMPRE
    private async void OnMenuClicked(object sender, EventArgs e)
    {
        string action = await Application.Current.MainPage.DisplayActionSheet(
            "Options",
            "Cancel",
            null,
            "🆕 CREATE DOCUMENT",
            "✅ POST DOCUMENT",
            "🧹 CLEAR",
            "❌ CLOSE"
        );

        switch (action)
        {
            case "🆕 CREATE DOCUMENT":
                // opcional
                break;

            case "✅ POST DOCUMENT":
                if (MenuCommand?.CanExecute(null) == true)
                    MenuCommand.Execute(null);
                break;

            case "🧹 CLEAR":
                break;

            case "❌ CLOSE":
                break;
        }
    }
}