using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiniMeLi.Services;
using MiniMeLi.Views;

namespace MiniMeLi.ViewModels;

public partial class CuentaViewModel : ObservableObject
{
    public string Nombre => AuthService.Nombre;
    public string Email => AuthService.Email;

    [RelayCommand]
    private async Task CerrarSesion()
    {
        bool ok = await Shell.Current.DisplayAlertAsync("Cerrar sesión", "¿Querés cerrar tu sesión?", "Sí", "No");
        if (!ok) return;

        AuthService.CerrarSesion();
        await Shell.Current.Navigation.PopAsync();
    }

    [RelayCommand]
    private async Task IrAMisPedidos()
    {
        await Shell.Current.Navigation.PushAsync(new MisPedidosPage());
    }
}
