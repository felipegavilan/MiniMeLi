using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiniMeLi.Models;
using MiniMeLi.Services;
using MiniMeLi.Views;

namespace MiniMeLi.ViewModels;

public partial class PedidoCreadoViewModel : ObservableObject
{
    private readonly PedidoCreado _pedido;
    private readonly List<string> _lineas;

    public string Titulo => $"¡Pedido #{_pedido.IdPedido} creado!";
    public decimal Total => _pedido.Total;
    public string NombreComercio => AppConfig.NombreComercio;
    public string AliasPago => AppConfig.AliasPago;

    public PedidoCreadoViewModel(PedidoCreado pedido, List<string> lineas)
    {
        _pedido = pedido;
        _lineas = lineas;
    }

    [RelayCommand]
    private async Task CopiarAlias()
    {
        await Clipboard.Default.SetTextAsync(AppConfig.AliasPago);
        await Shell.Current.DisplayAlertAsync("Listo", "Alias copiado.", "OK");
    }

    [RelayCommand]
    private async Task EnviarWhatsApp()
    {
        await WhatsAppService.EnviarPedidoAsync(_pedido.IdPedido, _lineas, _pedido.Total);
    }

    [RelayCommand]
    private async Task VolverAlCatalogo()
    {
        await Shell.Current.Navigation.PopToRootAsync();
    }

    [RelayCommand]
    private async Task VerMisPedidos()
    {
        await Shell.Current.Navigation.PushAsync(new MisPedidosPage());
    }
}