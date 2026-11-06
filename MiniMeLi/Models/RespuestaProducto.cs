using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMeLi.Models
{
    public class RespuestaProductos
    {
        public string Mensaje { get; set; } = string.Empty;
        public List<Producto> Response { get; set; } = new();
    }
}
