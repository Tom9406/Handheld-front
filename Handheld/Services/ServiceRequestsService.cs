using Handheld.Models;

namespace Handheld.Services;

public class ServiceRequestsService
{
    private readonly ApiService _api;

    public ServiceRequestsService(ApiService api)
    {
        _api = api;
    }

    public async Task<List<ServiceRequestDto>> GetMyRequests()
    {
        return await _api.GetAsync<List<ServiceRequestDto>>("api/servicerequests/my") ?? new List<ServiceRequestDto>();
    }

    public async Task<ServiceRequestDto?> GetById(int id)
    {
        return await _api.GetAsync<ServiceRequestDto>($"api/servicerequests/{id}");
    }

    public async Task<string> UploadPaymentProof(string filePath)
    {
        var response = await _api.PostMultipartAsync<UploadPaymentResponse>("api/uploads/payment", filePath, "file");
        return response?.ImageUrl ?? throw new InvalidOperationException("No se pudo subir el comprobante.");
    }

    public async Task<long> CreateRequest(int serviceId, string paymentImageUrl)
    {
        var response = await _api.PostAsync<CreateServiceRequestResponse>(
            "api/servicerequests",
            new CreateServiceRequestRequest
            {
                ServiceID = serviceId,
                ImageUrl = paymentImageUrl
            });

        return response?.RequestId ?? throw new InvalidOperationException("No se pudo crear la solicitud.");
    }

    public async Task UploadAttachment(long requestId, string filePath)
    {
        await _api.PostMultipartAsync<AttachmentResponse>($"api/servicerequests/{requestId}/attachment", filePath, "file");
    }

    public async Task CancelRequest(long requestId)
    {
        await _api.PutAsync<object>($"api/servicerequests/{requestId}/status", new
        {
            newStatus = "CANCELADO_USUARIO"
        });
    }
}
