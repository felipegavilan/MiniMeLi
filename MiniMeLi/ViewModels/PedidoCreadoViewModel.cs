using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiniMeLi.Models;
using MiniMeLi.Services;

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
        var mensaje = new StringBuilder();
        mensaje.AppendLine($"Hola, soy {AuthService.Nombre}. Hice el pedido #{_pedido.IdPedido} en {AppConfig.NombreComercio}:");
        mensaje.AppendLine();
        foreach (var linea in _lineas)
            mensaje.AppendLine($"• {linea}");
        mensaje.AppendLine();
        mensaje.AppendLine($"Total: {_pedido.Total:C}");
        mensaje.Append("Te envío el comprobante de pago.");

        var numero = new string(AppConfig.WhatsAppComercio.Where(char.IsDigit).ToArray());
        var url = $"https://wa.me/{numero}?text={Uri.EscapeDataString(mensaje.ToString())}";

        try
        {
            await Launcher.Default.OpenAsync(new Uri(url));
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlertAsync("No se pudo abrir WhatsApp",
                $"Escribile al comercio al {AppConfig.WhatsAppComercio} indicando tu número de pedido (#{_pedido.IdPedido}).", "OK");
        }
    }

    [RelayCommand]
    private async Task VolverAlCatalogo()
    {
        await Shell.Current.Navigation.PopToRootAsync();
    }
}