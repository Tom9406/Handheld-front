using Handheld.Models;
using Handheld.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Handheld.Views;

[QueryProperty(nameof(Line), "Line")]
public partial class ReceivingLineDetailsPage : ContentPage, INotifyPropertyChanged
{
    private ReceivingLineDto _line;
    private readonly ReceivingService _service;

    private decimal _qtyToReceive;

    public decimal QtyToReceive
    {
        get => _qtyToReceive;
        set
        {
            _qtyToReceive = value;
            OnPropertyChanged();
        }
    }

    //  Propiedades expuestas al XAML
    public string ItemCode => Line?.ItemCode;

    public decimal ExpectedQty => Line?.QuantityExpected ?? 0;

    public decimal AlreadyReceived => Line?.QuantityReceived ?? 0;

    public string UOM => Line?.UOM;

    public ReceivingLineDto Line
    {
        get => _line;
        set
        {
            _line = value;

            BindingContext = this;

            QtyToReceive = 0;

            OnPropertyChanged(nameof(ItemCode));            
            OnPropertyChanged(nameof(ExpectedQty));
            OnPropertyChanged(nameof(AlreadyReceived));
            OnPropertyChanged(nameof(UOM));
        }
    }

    public ReceivingLineDetailsPage(ReceivingService service)
    {
        InitializeComponent();
        _service = service;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            var newTotal = Line.QuantityReceived + QtyToReceive;

            // 🔒 Validación
            if (newTotal > Line.QuantityExpected)
            {
                await DisplayAlert(
                    "Error",
                    "Cannot receive more than expected.",
                    "OK");
                return;
            }

            if (QtyToReceive <= 0)
            {
                await DisplayAlert(
                    "Error",
                    "Enter a valid quantity.",
                    "OK");
                return;
            }

            var dto = new UpdateReceivingLineDto
            {
                QuantityReceived = newTotal
            };

            await _service.UpdateReceivingLineAsync(Line.Id, dto);

            await DisplayAlert("Success", "Line updated", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}