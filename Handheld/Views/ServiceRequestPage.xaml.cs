using System.Runtime.CompilerServices;
using Handheld.Models;
using Handheld.Services;

namespace Handheld.Views;

[QueryProperty(nameof(ServiceId), "serviceId")]
public partial class ServiceRequestPage : ContentPage
{
    private readonly ServicesService _servicesService;
    private readonly ServiceRequestsService _requestsService;

    private ServiceDto? _service;
    private string? _attachmentPath;
    private string? _paymentPath;

    public int ServiceId { get; set; }

    public string FileName { get; set; } = "Ningun archivo seleccionado";
    public string PaymentFileName { get; set; } = "Ningun comprobante seleccionado";

    public ServiceRequestPage(ServicesService servicesService, ServiceRequestsService requestsService)
    {
        InitializeComponent();
        _servicesService = servicesService;
        _requestsService = requestsService;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (ServiceId > 0)
            await LoadService(ServiceId);
    }

    private async Task LoadService(int id)
    {
        _service = await _servicesService.GetServiceById(id);

        if (_service == null)
        {
            await DisplayAlert("Error", "No se pudo cargar el servicio.", "OK");
            return;
        }

        BindingContext = this;
        RaisePropertyChanged(nameof(Name));
        RaisePropertyChanged(nameof(Category));
        RaisePropertyChanged(nameof(Description));
        RaisePropertyChanged(nameof(EstimatedTimeText));
        RaisePropertyChanged(nameof(Price));
        RaisePropertyChanged(nameof(PermiteAdjunto));
    }

    public string Name => _service?.Name ?? string.Empty;
    public string? Category => _service?.Category;
    public string? Description => _service?.Description;
    public string? EstimatedTimeText => _service?.EstimatedTimeText;
    public decimal Price => _service?.Price ?? 0;
    public bool PermiteAdjunto => _service?.PermiteAdjunto ?? false;

    private async void OnRequestClicked(object sender, EventArgs e)
    {
        if (_service == null)
            return;

        if (string.IsNullOrWhiteSpace(_paymentPath))
        {
            await DisplayAlert("Comprobante requerido", "Debes adjuntar la foto del comprobante de pago.", "OK");
            return;
        }

        try
        {
            var paymentImageUrl = await _requestsService.UploadPaymentProof(_paymentPath);
            var requestId = await _requestsService.CreateRequest(_service.ServiceID, paymentImageUrl);

            if (_service.PermiteAdjunto && !string.IsNullOrWhiteSpace(_attachmentPath))
                await _requestsService.UploadAttachment(requestId, _attachmentPath);

            await DisplayAlert("Solicitud enviada", "Tu servicio fue solicitado correctamente.", "OK");
            await Shell.Current.GoToAsync("//myrequests");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnPickFileClicked(object sender, EventArgs e)
    {
        var customTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.Android, new[] { "image/*", "application/pdf" } }
        });

        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Selecciona un archivo de respaldo",
            FileTypes = customTypes
        });

        if (result == null)
            return;

        var fileInfo = new FileInfo(result.FullPath);
        if (fileInfo.Length > 10 * 1024 * 1024)
        {
            await DisplayAlert("Archivo invalido", "El archivo supera los 10 MB.", "OK");
            return;
        }

        _attachmentPath = result.FullPath;
        FileName = result.FileName;
        RaisePropertyChanged(nameof(FileName));
    }

    private async void OnPickPaymentFileClicked(object sender, EventArgs e)
    {
        FileResult? result;

        if (MediaPicker.Default.IsCaptureSupported)
        {
            result = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Selecciona la foto del comprobante"
            });
        }
        else
        {
            result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona el comprobante",
                FileTypes = FilePickerFileType.Images
            });
        }

        if (result == null)
            return;

        var fileInfo = new FileInfo(result.FullPath);
        if (fileInfo.Length > 10 * 1024 * 1024)
        {
            await DisplayAlert("Archivo invalido", "La imagen supera los 10 MB.", "OK");
            return;
        }

        _paymentPath = result.FullPath;
        PaymentFileName = result.FileName;
        RaisePropertyChanged(nameof(PaymentFileName));
    }

    private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
    }
}
