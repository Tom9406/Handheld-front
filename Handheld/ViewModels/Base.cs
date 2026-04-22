using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Handheld.ViewModels.Base
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        bool isLoading;
        bool hasError;
        string errorMessage = string.Empty;

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                if (SetProperty(ref isLoading, value))
                {
                    OnPropertyChanged(nameof(IsEmpty));
                }
            }
        }

        public bool HasError
        {
            get => hasError;
            set
            {
                if (SetProperty(ref hasError, value))
                {
                    OnPropertyChanged(nameof(IsEmpty));
                }
            }
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        public virtual bool IsEmpty => false;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(
            ref T backingStore,
            T value,
            [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
