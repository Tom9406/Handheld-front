using Handheld.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Handheld.ViewModels;

public class HomeViewModel : BaseViewModel
{
    public ObservableCollection<CategoryItem> Categories { get; set; }

    public ICommand GoToCategoryCommand { get; }

    public HomeViewModel()
    {
        Categories = new ObservableCollection<CategoryItem>
{
    new CategoryItem { Title = "Legales", Icon = "icon_documents.svg", Type = "services", BackendCategory = "DOCUMENTOS" },
    new CategoryItem { Title = "Compras", Icon = "icon_purchase.svg", Type = "services", BackendCategory = "COMPRAS" },
    new CategoryItem { Title = "Documentos", Icon = "icon_folder.svg", Type = "services", BackendCategory = "DOCUMENTOS" },
    new CategoryItem { Title = "Delivery", Icon = "icon_delivery.svg", Type = "services", BackendCategory = "DELIVERY" },
    new CategoryItem { Title = "Otros", Icon = "icon_settings.svg", Type = "services", BackendCategory = "OTROS" },
    new CategoryItem { Title = "Mis Solicitudes", Icon = "icon_receipt.svg", Type = "historial" }
};

        GoToCategoryCommand = new Command<CategoryItem>(async (item) =>
        {
            if (item == null)
                return;

            if (item.Type == "historial")
            {
                await Shell.Current.GoToAsync("//myrequests");
                return;
            }

            if (item.Type == "services")
            {
                await Shell.Current.GoToAsync($"//services?category={item.BackendCategory}");
                return;
            }
        });
    }
}
