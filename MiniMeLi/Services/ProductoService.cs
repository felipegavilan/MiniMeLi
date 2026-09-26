using System.Collections.Generic;
using MiniMeLi.Models;

namespace MiniMeLi.Services
{
    public static class ProductoService
    {
        public static List<Producto> ObtenerProductosPrueba()
        {
            return new List<Producto>
            {
                new Producto
                {
                    Id = 1,
                    Titulo = "Smartphone Gamer 128GB",
                    Precio = 450000m,
                    Descripcion = "Teléfono de alta gama con procesador optimizado para juegos.",
                    ImagenUrl = "dotnet_bot.png",
                    Stock = 10
                },
                new Producto
                {
                    Id = 2,
                    Titulo = "Auriculares Inalámbricos Bluetooth",
                    Precio = 65000m,
                    Descripcion = "Sonido envolvente estéreo con cancelación de ruido pasiva.",
                    ImagenUrl = "dotnet_bot.png",
                    Stock = 25
                },
                new Producto
                {
                    Id = 3,
                    Titulo = "Notebook Mecánica RGB",
                    Precio = 980000m,
                    Descripcion = "Teclado mecánico retroiluminado ideal para desarrollo y gaming.",
                    ImagenUrl = "dotnet_bot.png",
                    Stock = 5
                },
                new Producto
                {
                    Id = 4,
                    Titulo = "Mouse Ergonómico Inalámbrico",
                    Precio = 32000m,
                    Descripcion = "Diseño cómodo para largas horas de uso en la computadora.",
                    ImagenUrl = "dotnet_bot.png",
                    Stock = 15
                }
            };
        }
    }
}