namespace MiniMeLi.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}