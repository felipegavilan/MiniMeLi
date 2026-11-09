namespace MiniMeLi.Models;

public class PedidoCreado
{
    public int IdPedido { get; set; }
    public string Estado { get; set; } = "";
    public decimal Total { get; set; }
    public DateTime Fecha { get; set; }
}