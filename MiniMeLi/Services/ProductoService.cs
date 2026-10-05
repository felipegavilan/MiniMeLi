using MiniMeLi.Models;
using System.Text.Json;

namespace MiniMeLi.Services
{
    public static class ProductoService
    {
     public static async Task<List<Producto>> ObtenerProductosAsync()
        {
            // abrimos el archivo empaquetado en la app
            using var stream = await FileSystem.OpenAppPackageFileAsync("products.json");

            // convertimos a texto
            using var reader = new StreamReader(stream);
            string json = await reader.ReadToEndAsync();

            //convertimos el texto en objeto
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<Producto>>(json, opciones) ?? new List<Producto>();

        }
    }
}