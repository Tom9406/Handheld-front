using Handheld.Models;
using Handheld.Services;

namespace Handheld.Views;

[QueryProperty(nameof(RequestId), "id")]
public partial class MyRequestDetails : ContentPage
{
    private readonly ServiceRequestsService _service;
    private ServiceRequestDto? _request;
    private bool _isLoaded;

    public int RequestId { get; set; }

    public MyRequestDetails(ServiceRequestsService service)
    {
        InitializeComponent();
        _service = service;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (RequestId <= 0 || _isLoaded)
            return;

        _isLoaded = true;
        await LoadData();
    }

    private async Task LoadData()
    {
        try
        {
            _request = await _service.GetById(RequestId);

            if (_request == null)
                return;

            BindingContext = new
            {
                _request.ServiceName,
                _request.Status,
                _request.PaymentStatus,
                _request.Category,
                _request.EstimatedTimeText,
                _request.Price,
                _request.FirstName,
                _request.LastName,
                _request.Phone,
                _request.Email,
                _request.CreatedAt,
                _request.UpdatedAt,
                _request.ValidatedAt,
                CanCancel = _request.Status == "SOLICITADO"
            };
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        if (_request == null)
            return;

        var confirm = await DisplayAlert("Cancelar", "¿Deseas cancelar esta solicitud?", "Sí", "No");
        if (!confirm)
            return;

        try
        {
            await _service.CancelRequest(_request.RequestID);
            await DisplayAlert("Listo", "La solicitud fue cancelada.", "OK");
            await Shell.Current.GoToAsync("//myrequests");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
