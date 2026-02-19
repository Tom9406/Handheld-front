using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Handheld.Models;
using Handheld.Services;

namespace Handheld.ViewModels;

public class CompanyViewModel : INotifyPropertyChanged
{
    private readonly CompanyService _companyService;

    public CompanyViewModel(CompanyService companyService)
    {
        _companyService = companyService;
        CreateCompanyCommand = new Command(async () => await CreateCompanyAsync(), () => !IsBusy);
    }

    #region Properties

    private string _code = string.Empty;
    public string Code
    {
        get => _code;
        set => SetProperty(ref _code, value);
    }

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string _currencyCode = string.Empty;
    public string CurrencyCode
    {
        get => _currencyCode;
        set => SetProperty(ref _currencyCode, value);
    }

    private string _timeZone = string.Empty;
    public string TimeZone
    {
        get => _timeZone;
        set => SetProperty(ref _timeZone, value);
    }

    private string? _companyType;
    public string? CompanyType
    {
        get => _companyType;
        set => SetProperty(ref _companyType, value);
    }

    private string? _legalName;
    public string? LegalName
    {
        get => _legalName;
        set => SetProperty(ref _legalName, value);
    }

    private string? _taxId;
    public string? TaxId
    {
        get => _taxId;
        set => SetProperty(ref _taxId, value);
    }

    private string? _address1;
    public string? Address1
    {
        get => _address1;
        set => SetProperty(ref _address1, value);
    }

    private string? _city;
    public string? City
    {
        get => _city;
        set => SetProperty(ref _city, value);
    }

    private string? _country;
    public string? Country
    {
        get => _country;
        set => SetProperty(ref _country, value);
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
                ((Command)CreateCompanyCommand).ChangeCanExecute();
        }
    }

    #endregion

    public ICommand CreateCompanyCommand { get; }

    private async Task CreateCompanyAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            if (string.IsNullOrWhiteSpace(Code) ||
                string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(CurrencyCode) ||
                string.IsNullOrWhiteSpace(TimeZone))
                        {
                            await Application.Current.MainPage.DisplayAlert(
                                "Validation",
                                "Required fields missing.",
                                "OK");

                return;
            }


            var request = new CreateCompanyRequest
            {
                Code = Code,
                Name = Name,
                CurrencyCode = CurrencyCode,
                TimeZone = TimeZone,
                CompanyType = CompanyType,
                LegalName = LegalName,
                TaxId = TaxId,
                Address1 = Address1,
                City = City,
                Country = Country
            };

            var result = await _companyService.CreateCompanyAsync(request);

            await Application.Current.MainPage.DisplayAlert(
                "Success",
                $"Company created: {result.Name}",
                "OK");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }

    #endregion
}
