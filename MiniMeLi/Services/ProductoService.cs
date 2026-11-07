using MiniMeLi.Models;
using Newtonsoft.Json;

namespace MiniMeLi.Services;

public class ProductoService
{
    // static: un solo HttpClient para toda la app. Timeout largo porque somee puede tardar al "despertar"
    private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(30) };

    public async Task<List<Producto>> ObtenerProductosAsync()
    {
        var response = await _httpClient.GetAsync($"{AppConfig.ApiBaseUrl}producto/lista");
        response.EnsureSuccessStatusCode(); // si falla, lanza excepción y la maneja el ViewModel

        var json = await response.Content.ReadAsStringAsync();
        var resultado = JsonConvert.DeserializeObject<RespuestaProductos>(json);
        return resultado?.Response ?? new List<Producto>();
    }
}