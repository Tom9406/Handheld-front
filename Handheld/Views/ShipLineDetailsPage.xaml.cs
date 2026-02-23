using Handheld.Models;
using Handheld.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Handheld.Views;

[QueryProperty(nameof(Line), "Line")]
public partial class ShipLineDetailsPage : ContentPage, INotifyPropertyChanged
{
    private ShipmentLineDto _line;
    private readonly ShipmentService _service;

    private decimal _qtyToAdd;

    public decimal QtyToAdd
    {
        get => _qtyToAdd;
        set
        {
            _qtyToAdd = value;
            OnPropertyChanged();
        }
    }

    // 🔹 Exponemos propiedades del DTO para que el XAML las vea
    public string ItemNo => Line?.ItemNo;

    public decimal OrderedQty => Line?.OrderedQty ?? 0;

    public decimal AlreadyShipped => Line?.ShippedQty ?? 0;

    public ShipmentLineDto Line
    {
        get => _line;
        set
        {
            _line = value;

            BindingContext = this;

            QtyToAdd = 0;

            // Notificar que las propiedades visibles cambiaron
            OnPropertyChanged(nameof(ItemNo));
            OnPropertyChanged(nameof(OrderedQty));
            OnPropertyChanged(nameof(AlreadyShipped));
        }
    }

    public ShipLineDetailsPage(ShipmentService service)
    {
        InitializeComponent();
        _service = service;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            var newTotal = Line.ShippedQty + QtyToAdd;

            var dto = new UpdateShipmentLineDto
            {
                ShippedQty = newTotal
            };

            await _service.UpdateShipmentLineAsync(Guid.Parse(Line.Id), dto);

            await DisplayAlert("Success", "Line updated", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}