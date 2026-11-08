using System.Net.Mail;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiniMeLi.Services;

namespace MiniMeLi.ViewModels;

public partial class RegistroViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegistrarseCommand))]
    private string _nombre = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegistrarseCommand))]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _telefono = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegistrarseCommand))]
    private string _password = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegistrarseCommand))]
    private string _confirmarPassword = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegistrarseCommand))]
    private bool _isBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HayError))]
    private string _mensajeError = string.Empty;

    public bool HayError => !string.IsNullOrEmpty(MensajeError);

    private bool PuedeEnviar() =>
        !IsBusy
        && !string.IsNullOrWhiteSpace(Nombre)
        && !string.IsNullOrWhiteSpace(Email)
        && !string.IsNullOrEmpty(Password)
        && !string.IsNullOrEmpty(ConfirmarPassword);

    [RelayCommand(CanExecute = nameof(PuedeEnviar))]
    private async Task Registrarse()
    {
        MensajeError = string.Empty;

        if (!MailAddress.TryCreate(Email.Trim(), out _))
        {
            MensajeError = "El email no es válido.";
            return;
        }
        if (Password.Length < 6)
        {
            MensajeError = "La contraseña debe tener al menos 6 caracteres.";
            return;
        }
        if (Password != ConfirmarPassword)
        {
            MensajeError = "Las contraseñas no coinciden.";
            return;
        }

        IsBusy = true;
        var (ok, mensaje) = await AuthService.RegistroAsync(Nombre, Email, Telefono, Password);
        IsBusy = false;

        if (!ok)
        {
            MensajeError = mensaje;
            return;
        }
        // El registro ya deja la sesión iniciada: volvemos al catálogo
        await Shell.Current.Navigation.PopToRootAsync();
    }
}