using Microsoft.Maui.Graphics;

namespace MiniMeLi.Models;

public class PedidoLinea
{
    public int IdProd { get; set; }
    public string? Descripcion { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

public class PedidoResumen
{
    public int IdPedido { get; set; }
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = "";
    public decimal Total { get; set; }
    public DateTime? FechaConfirmacion { get; set; }
    public List<PedidoLinea> Items { get; set; } = new();

    // Calculadas para mostrar en pantalla
    public string Titulo => $"Pedido #{IdPedido}";
    public string FechaTexto => Fecha.ToString("dd/MM/yyyy HH:mm");
    public bool EsPendiente => Estado == "Pendiente";

    public string TextoEstado => Estado switch
    {
        "Pendiente" => "Pendiente de pago",
        "Confirmado" => "Confirmado",
        "Cancelado" => "Cancelado",
        _ => Estado
    };

    public Color ColorEstado => Estado switch
    {
        "Confirmado" => Color.FromArgb("#2E7D32"),
        "Cancelado" => Color.FromArgb("#9E9E9E"),
        _ => Color.FromArgb("#E65100")
    };

    public List<string> Lineas =>
        Items.Select(i => $"{i.Cantidad} x {i.Descripcion ?? "Producto"}").ToList();

    public string Detalle => string.Join("\n", Lineas);
}

public class RespuestaPedidos
{
    public string Mensaje { get; set; } = "";
    public List<PedidoResumen> Response { get; set; } = new();
}