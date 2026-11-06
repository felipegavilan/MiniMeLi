using MiniMeLi.Models;
using Newtonsoft.Json;

namespace MiniMeLi.Services
{
    public class ProductoService
    {
        private readonly HttpClient _httpClient = new HttpClient();

    public async Task<List<Producto>> ObtenerProductosAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("http://apidispomovil.somee.com/api/producto/lista");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<RespuestaProductos>(jsonResponse);
                return resultado?.Response ?? new List<Producto>();
            }
            return new List<Producto>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching products: {ex.Message}");
            return new List<Producto>();
        }
    }
}
}