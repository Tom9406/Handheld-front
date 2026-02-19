using Android.Telephony;
using Handheld.Models;
using Handheld.Services;
using Handheld.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static Android.Icu.Text.CaseMap;

namespace Handheld.ViewModels;

public class PickHeadersViewModel : BaseViewModel
{
    private readonly PickService _pickService;

    public ObservableCollection<PickHeaderDto> Items { get; } = new();

    public ICommand LoadCommand { get; }

    private int _pageNumber = 1;
    private const int PageSize = 20;

    public PickHeadersViewModel(PickService pickService)
    {
        _pickService = pickService;

        

        LoadCommand = new Command(async () => await LoadAsync());
    }

    public bool HasData => Items.Count > 0;
    public bool IsEmpty => !IsLoading && Items.Count == 0;

    public async Task InitializeAsync()
    {
        if (Items.Count == 0)
            await LoadAsync();
    }

    public async Task LoadAsync()
    {
        if (IsLoading)
            return;

        try
        {
            IsLoading = true;
            HasError = false;
            ErrorMessage = string.Empty;

            var response = await _pickService.GetPicksAsync(_pageNumber, PageSize);

            Items.Clear();

            if (response?.Data != null)
            {
                foreach (var item in response.Data)
                    Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;

            OnPropertyChanged(nameof(HasData));
            OnPropertyChanged(nameof(IsEmpty));
        }
    }
}
