using System.Net;
using System.Text;
using MiniMeLi.Models;
using Newtonsoft.Json;

namespace MiniMeLi.Services;

public static class PedidoService
{
    public static async Task<(bool Ok, string Mensaje, PedidoCreado? Pedido)> CrearPedidoAsync(IEnumerable<CartItem> items)
    {
        try
        {
            // Solo mandamos producto y cantidad: el precio lo calcula el servidor
            var cuerpo = new
            {
                Items = items.Select(i => new { IdProd = i.Producto.IdProd, Cantidad = i.Cantidad }).ToList()
            };
            var contenido = new StringContent(JsonConvert.SerializeObject(cuerpo), Encoding.UTF8, "application/json");

            var respuesta = await AuthService.Http.PostAsync(AppConfig.ApiBaseUrl + "pedido", contenido);
            var json = await respuesta.Content.ReadAsStringAsync();

            if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
            {
                AuthService.CerrarSesion();
                return (false, "Tu sesión venció. Iniciá sesión de nuevo para finalizar la compra.", null);
            }

            if (!respuesta.IsSuccessStatusCode)
                return (false, AuthService.LeerMensaje(json) ?? $"Error del servidor ({(int)respuesta.StatusCode}).", null);

            var pedido = JsonConvert.DeserializeObject<PedidoCreado>(json);
            if (pedido == null || pedido.IdPedido <= 0)
                return (false, "Respuesta inválida del servidor.", null);

            return (true, "", pedido);
        }
        catch (TaskCanceledException)
        {
            return (false, "El servidor tardó demasiado en responder. Revisá tus pedidos antes de reintentar.", null);
        }
        catch (HttpRequestException)
        {
            return (false, "No pudimos conectar con el servidor. Revisá tu conexión.", null);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al crear el pedido: {ex.Message}");
            return (false, "Ocurrió un error inesperado. Intentá de nuevo.", null);
        }
    }
    public static async Task<(bool Ok, string Mensaje, List<PedidoResumen> Pedidos, bool SesionVencida)> ObtenerMisPedidosAsync()
    {
        try
        {
            var respuesta = await AuthService.Http.GetAsync(AppConfig.ApiBaseUrl + "pedido/mios");

            if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
            {
                AuthService.CerrarSesion();
                return (false, "", new List<PedidoResumen>(), true);
            }

            var json = await respuesta.Content.ReadAsStringAsync();

            if (!respuesta.IsSuccessStatusCode)
                return (false, AuthService.LeerMensaje(json) ?? $"Error del servidor ({(int)respuesta.StatusCode}).",
                        new List<PedidoResumen>(), false);

            var datos = JsonConvert.DeserializeObject<RespuestaPedidos>(json);
            return (true, "", datos?.Response ?? new List<PedidoResumen>(), false);
        }
        catch (TaskCanceledException)
        {
            return (false, "El servidor tardó demasiado en responder. Intentá de nuevo.", new List<PedidoResumen>(), false);
        }
        catch (HttpRequestException)
        {
            return (false, "No pudimos conectar con el servidor. Revisá tu conexión.", new List<PedidoResumen>(), false);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al obtener pedidos: {ex.Message}");
            return (false, "Ocurrió un error inesperado. Intentá de nuevo.", new List<PedidoResumen>(), false);
        }
    }

    public static async Task<(bool Ok, string Mensaje)> CancelarPedidoAsync(int idPedido)
    {
        try
        {
            var respuesta = await AuthService.Http.PutAsync($"{AppConfig.ApiBaseUrl}pedido/cancelar/{idPedido}", null);

            if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
            {
                AuthService.CerrarSesion();
                return (false, "Tu sesión venció. Iniciá sesión de nuevo.");
            }

            if (!respuesta.IsSuccessStatusCode)
            {
                var json = await respuesta.Content.ReadAsStringAsync();
                return (false, AuthService.LeerMensaje(json) ?? $"Error del servidor ({(int)respuesta.StatusCode}).");
            }

            return (true, "");
        }
        catch (TaskCanceledException)
        {
            return (false, "El servidor tardó demasiado en responder. Intentá de nuevo.");
        }
        catch (HttpRequestException)
        {
            return (false, "No pudimos conectar con el servidor. Revisá tu conexión.");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al cancelar el pedido: {ex.Message}");
            return (false, "Ocurrió un error inesperado. Intentá de nuevo.");
        }
    }
}