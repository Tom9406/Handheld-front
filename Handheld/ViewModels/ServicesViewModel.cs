using Handheld.Models;
using Handheld.Services;
using Handheld.ViewModels.Base;
using IntelliJ.Lang.Annotations;
using System.Collections.ObjectModel;

namespace Handheld.ViewModels;

public class ServicesViewModel : BaseViewModel
{
    private readonly ServicesService _service;

    public ObservableCollection<ServiceDto> Services { get; set; } = new();

    public ServicesViewModel(ServicesService service)
    {
        _service = service;
    }

    public async Task LoadServicesByCategory(string category)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            var data = await _service.GetByCategory(category);

            Services.Clear();

            foreach (var item in data)
                Services.Add(item);
        }
        finally
        {
            IsBusy = false;
        }
    }
}