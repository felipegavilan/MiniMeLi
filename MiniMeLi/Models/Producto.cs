using Newtonsoft.Json;

namespace MiniMeLi.Models
{
    public class Categoria
    {
        public int IdCat { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    public class Producto
    {
        private const int UmbralStockBajo = 5;

        public int IdProd { get; set; }
        public string CodigoBarra { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public int IdCat { get; set; }
        public decimal Precio { get; set; }
        public string? ImagenUrl { get; set; }
        public Categoria? OCategoria { get; set; }
        public int Stock { get; set; }

        // ---- Calculadas para la pantalla ----
        [JsonIgnore] public bool SinStock => Stock <= 0;
        [JsonIgnore] public bool HayStock => Stock > 0;
        [JsonIgnore] public bool MostrarAvisoStock => Stock <= UmbralStockBajo;
        [JsonIgnore] public bool StockNormal => Stock > UmbralStockBajo;

        [JsonIgnore]
        public string TextoStock =>
            Stock <= 0 ? "Sin stock" :
            Stock == 1 ? "¡Última unidad!" :
            Stock <= UmbralStockBajo ? $"¡Quedan {Stock} unidades!" : "";

        [JsonIgnore] public string TextoBotonCompra => HayStock ? "Comprar ahora" : "Sin stock";
    }
}