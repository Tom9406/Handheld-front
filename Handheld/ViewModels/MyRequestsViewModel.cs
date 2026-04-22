using Handheld.Models;
using Handheld.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Handheld.ViewModels;

public class MyRequestsViewModel : INotifyPropertyChanged
{
    private readonly ServiceRequestsService _service;

    public ObservableCollection<ServiceRequestDto> Requests { get; set; } = new();

    private List<ServiceRequestDto> _allRequests = new();

    public bool IsLoading { get; set; }

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText == value)
                return;

            _searchText = value;
            FilterRequests();
        }
    }

    public MyRequestsViewModel(ServiceRequestsService service)
    {
        _service = service;
    }

    public async Task LoadRequests()
    {
        if (IsLoading)
            return;

        IsLoading = true;

        try
        {
            var data = await _service.GetMyRequests();

            _allRequests = data
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            FilterRequests();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR MyRequests: {ex}");

            if (Application.Current?.MainPage != null)
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void FilterRequests()
    {
        IEnumerable<ServiceRequestDto> filtered;

        if (string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = _allRequests;
        }
        else
        {
            var search = SearchText.Trim().ToLowerInvariant();

            filtered = _allRequests.Where(x =>
                (x.ServiceName?.ToLowerInvariant().Contains(search) ?? false) ||
                (x.Status?.ToLowerInvariant().Contains(search) ?? false) ||
                (x.Category?.ToLowerInvariant().Contains(search) ?? false) ||
                (x.PaymentStatus?.ToLowerInvariant().Contains(search) ?? false) ||
                (x.EstimatedTimeText?.ToLowerInvariant().Contains(search) ?? false)
            );
        }

        Requests = new ObservableCollection<ServiceRequestDto>(filtered);
        OnPropertyChanged(nameof(Requests));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string name)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
