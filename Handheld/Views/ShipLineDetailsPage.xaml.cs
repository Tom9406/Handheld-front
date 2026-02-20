using System;
using Handheld.Models; 

namespace Handheld.Views;

[QueryProperty(nameof(Line), "Line")]
public partial class ShipLineDetailsPage : ContentPage
{
    private ShipmentLineDto _line;

    public ShipmentLineDto Line
    {
        get => _line;
        set
        {
            _line = value;
            BindingContext = _line;
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    public ShipLineDetailsPage()
    {
        InitializeComponent();
    }
}