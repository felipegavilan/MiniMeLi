namespace MiniMeLi.Models
{
    public class Categoria
    {
        public int IdCat { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
    public class Producto
    {
        public int IdProd { get; set; }
        public string CodigoBarra { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public int IdCat { get; set; }
        public decimal Precio { get; set; }
        public string? ImagenUrl { get; set; }
        public Categoria? OCategoria { get; set; }
    }
}