using System.Windows.Input;

namespace Handheld.ViewModels;

public class MainViewModel
{
    // Define las propiedades de los comandos aquí
    public ICommand GoPickCommand { get; }

    public ICommand GoItemCommand { get; }

    public ICommand GoMovementCommand { get; }

    public ICommand GoReceiveCommand { get; }

    public ICommand GoShipCommand { get; }

    public ICommand GoRegisterCompanyCommand { get; }


    public MainViewModel()
    {
        // Inicializa el comando dentro del ViewModel
        // Verifica que la ruta "Name_Page" esté registrada en AppShell.xaml.cs

        GoPickCommand = new Command(async () =>
        {
            
            await Shell.Current.GoToAsync("PickingPage");
        });


        GoItemCommand = new Command(async () =>
        {
            
            await Shell.Current.GoToAsync("ItemInquiryPage");
        });

        GoMovementCommand = new Command(async () =>
        {
            
            await Shell.Current.GoToAsync("MovementsPage");
        });

        
        GoReceiveCommand = new Command(async () =>
        {
            
            await Shell.Current.GoToAsync("ReceivingPage");
        });

        GoShipCommand = new Command(async () =>
        {

            await Shell.Current.GoToAsync("ShipmentHeadersPage");
        });

        GoRegisterCompanyCommand = new Command(async () =>
        {

            await Shell.Current.GoToAsync("RegisterCompanyPage");
        });

        

    }
}