using System.Text;

namespace MiniMeLi.Services;

public static class WhatsAppService
{
    public static async Task EnviarPedidoAsync(int idPedido, IEnumerable<string> lineas, decimal total)
    {
        var mensaje = new StringBuilder();
        mensaje.AppendLine($"Hola, soy {AuthService.Nombre}. Hice el pedido #{idPedido} en {AppConfig.NombreComercio}:");
        mensaje.AppendLine();
        foreach (var linea in lineas)
            mensaje.AppendLine($"• {linea}");
        mensaje.AppendLine();
        mensaje.AppendLine($"Total: {total:C}");
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
                $"Escribile al comercio al {AppConfig.WhatsAppComercio} indicando tu número de pedido (#{idPedido}).", "OK");
        }
    }
}