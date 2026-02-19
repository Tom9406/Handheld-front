using Android.App;
using Android.Content.PM;
using Microsoft.Maui;

namespace Handheld;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    ConfigurationChanges =
        ConfigChanges.ScreenSize |
        ConfigChanges.Orientation |
        ConfigChanges.UiMode |
        ConfigChanges.ScreenLayout |
        ConfigChanges.SmallestScreenSize,
    ScreenOrientation = ScreenOrientation.Portrait
)]
public class MainActivity : MauiAppCompatActivity
{
}
