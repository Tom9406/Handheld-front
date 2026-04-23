using System.Runtime.CompilerServices;
using Handheld.Models;
using Handheld.Services;

namespace Handheld.Views;

[QueryProperty(nameof(ServiceId), "serviceId")]
public partial class ServiceRequestPage : ContentPage
{
    private const long MaxFileSizeBytes = 10 * 1024 * 1024;

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
        try
        {
            _service = await _servicesService.GetServiceById(id);
        }
        catch
        {
            await DisplayAlert("No se pudo cargar el servicio", "Verifica la conexion e intenta nuevamente.", "OK");
            await Shell.Current.GoToAsync("..");
            return;
        }

        if (_service == null)
        {
            await DisplayAlert("Servicio no disponible", "No se pudo encontrar el servicio seleccionado.", "OK");
            await Shell.Current.GoToAsync("..");
            return;
        }

        if (!_service.IsActive)
        {
            await DisplayAlert("Servicio no disponible", "Este servicio ya no esta activo para solicitar.", "OK");
            await Shell.Current.GoToAsync("..");
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
        {
            await DisplayAlert("Servicio requerido", "Selecciona un servicio disponible para continuar.", "OK");
            return;
        }

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

            await DisplayAlert("Solicitud enviada", "Tu solicitud fue registrada correctamente y el pago quedo pendiente de validacion.", "OK");
            await Shell.Current.GoToAsync("//home");
            await Shell.Current.GoToAsync("myrequests");
        }
        catch (Exception ex)
        {
            await DisplayAlert("No se pudo crear la solicitud", GetFriendlyError(ex), "OK");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnPickFileClicked(object sender, EventArgs e)
    {
        try
        {
            var customTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "image/png", "image/jpeg", "application/pdf" } }
            });

            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona un archivo de respaldo",
                FileTypes = customTypes
            });

            if (result == null)
                return;

            var preparedPath = await PreparePickedFile(result, isPaymentProof: false);
            if (string.IsNullOrWhiteSpace(preparedPath))
                return;

            _attachmentPath = preparedPath;
            FileName = result.FileName;
            RaisePropertyChanged(nameof(FileName));
        }
        catch
        {
            await DisplayAlert("No se pudo seleccionar el archivo", "Intenta nuevamente con un PDF, JPG o PNG menor a 10 MB.", "OK");
        }
    }

    private async void OnPickPaymentFileClicked(object sender, EventArgs e)
    {
        try
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

            var preparedPath = await PreparePickedFile(result, isPaymentProof: true);
            if (string.IsNullOrWhiteSpace(preparedPath))
                return;

            _paymentPath = preparedPath;
            PaymentFileName = result.FileName;
            RaisePropertyChanged(nameof(PaymentFileName));
        }
        catch
        {
            await DisplayAlert("No se pudo seleccionar el comprobante", "Intenta nuevamente con una imagen JPG o PNG menor a 10 MB.", "OK");
        }
    }

    private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
    }

    private async Task<string?> PreparePickedFile(FileResult result, bool isPaymentProof)
    {
        var extension = Path.GetExtension(result.FileName).ToLowerInvariant();
        var allowedExtensions = isPaymentProof
            ? new[] { ".jpg", ".jpeg", ".png" }
            : new[] { ".jpg", ".jpeg", ".png", ".pdf" };

        if (!allowedExtensions.Contains(extension))
        {
            var message = isPaymentProof
                ? "Selecciona una imagen JPG o PNG para el comprobante."
                : "Selecciona un archivo PDF, JPG o PNG.";

            await DisplayAlert("Formato no permitido", message, "OK");
            return null;
        }

        var cachePath = Path.Combine(FileSystem.CacheDirectory, $"{Guid.NewGuid()}{extension}");
        long totalBytes = 0;

        await using var source = await result.OpenReadAsync();
        await using var target = File.Create(cachePath);
        var buffer = new byte[81920];
        int read;

        while ((read = await source.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
        {
            totalBytes += read;

            if (totalBytes > MaxFileSizeBytes)
            {
                target.Close();
                File.Delete(cachePath);
                await DisplayAlert("Archivo muy grande", "El archivo no puede superar 10 MB.", "OK");
                return null;
            }

            await target.WriteAsync(buffer.AsMemory(0, read));
        }

        return cachePath;
    }

    private static string GetFriendlyError(Exception ex)
    {
        if (string.IsNullOrWhiteSpace(ex.Message))
            return "Revisa los datos e intenta nuevamente.";

        if (ex.Message.Contains("Connection failure", StringComparison.OrdinalIgnoreCase) ||
            ex.Message.Contains("timed out", StringComparison.OrdinalIgnoreCase) ||
            ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase))
        {
            return "No se pudo conectar con el servidor. Verifica la red y que la API este activa.";
        }

        return ex.Message;
    }
}
