using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiniMeLi.Models;
using MiniMeLi.Services;
using MiniMeLi.Views;

namespace MiniMeLi.ViewModels;

public partial class MisPedidosViewModel : ObservableObject
{
    public ObservableCollection<PedidoResumen> Pedidos { get; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MostrarLista))]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MostrarLista))]
    private bool _sinSesion;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HayError))]
    [NotifyPropertyChangedFor(nameof(MostrarLista))]
    private string _mensajeError = string.Empty;

    public bool HayError => !string.IsNullOrEmpty(MensajeError);
    public bool MostrarLista => !IsLoading && !SinSesion && !HayError;

    public async Task CargarAsync(bool desdeRefresh = false)
    {
        await AuthService.InicializarAsync();
        MensajeError = string.Empty;

        if (!AuthService.EstaLogueado)
        {
            Pedidos.Clear();
            SinSesion = true;
            IsRefreshing = false;
            return;
        }

        SinSesion = false;
        if (!desdeRefresh && Pedidos.Count == 0)
            IsLoading = true;

        var resultado = await PedidoService.ObtenerMisPedidosAsync();

        IsLoading = false;
        IsRefreshing = false;

        if (resultado.SesionVencida)
        {
            Pedidos.Clear();
            SinSesion = true;
            return;
        }

        if (!resultado.Ok)
        {
            MensajeError = resultado.Mensaje;
            return;
        }

        Pedidos.Clear();
        foreach (var pedido in resultado.Pedidos)
            Pedidos.Add(pedido);
    }

    [RelayCommand]
    private async Task Refrescar()
    {
        await CargarAsync(desdeRefresh: true);
    }

    [RelayCommand]
    private async Task Reintentar()
    {
        await CargarAsync();
    }

    [RelayCommand]
    private async Task Cancelar(PedidoResumen pedido)
    {
        bool ok = await Shell.Current.DisplayAlertAsync("Cancelar pedido",
            $"¿Querés cancelar el pedido #{pedido.IdPedido}?", "Sí, cancelar", "No");
        if (!ok) return;

        var resultado = await PedidoService.CancelarPedidoAsync(pedido.IdPedido);
        if (!resultado.Ok)
            await Shell.Current.DisplayAlertAsync("No se pudo cancelar", resultado.Mensaje, "OK");

        // Se recarga siempre, así se ve el estado real del pedido
        await CargarAsync(desdeRefresh: true);
    }

    [RelayCommand]
    private async Task EnviarComprobante(PedidoResumen pedido)
    {
        await WhatsAppService.EnviarPedidoAsync(pedido.IdPedido, pedido.Lineas, pedido.Total);
    }

    [RelayCommand]
    private async Task IrALogin()
    {
        await Shell.Current.Navigation.PushAsync(new LoginPage());
    }
}