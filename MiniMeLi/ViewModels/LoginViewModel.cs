using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiniMeLi.Services;
using MiniMeLi.Views;

namespace MiniMeLi.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(IniciarSesionCommand))]
    private string _email = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(IniciarSesionCommand))]
    private string _password = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(IniciarSesionCommand))]
    private bool _isBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HayError))]
    private string _mensajeError = string.Empty;

    public bool HayError => !string.IsNullOrEmpty(MensajeError);

    private bool PuedeEnviar() =>
        !IsBusy && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrEmpty(Password);

    [RelayCommand(CanExecute = nameof(PuedeEnviar))]
    private async Task IniciarSesion()
    {
        MensajeError = string.Empty;
        IsBusy = true;
        var (ok, mensaje) = await AuthService.LoginAsync(Email, Password);
        IsBusy = false;

        if (!ok)
        {
            MensajeError = mensaje;
            return;
        }
        await NavegacionAuth.VolverAlOrigenAsync();
    }

    [RelayCommand]
    private async Task IrARegistro()
    {
        await Shell.Current.Navigation.PushAsync(new RegistroPage());
    }
}