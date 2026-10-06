using System.Collections.ObjectModel;
using MiniMeLi.Models;

namespace MiniMeLi.Services;

public static class CarritoService
{
    public static ObservableCollection<Producto> Items { get; } = new ();

    public static void AgregarProducto(Producto producto)
    {
        Items.Add(producto);
    }

    public static void RemoverProducto(Producto producto)
    {
        Items.Remove(producto);
    }

    //se calcula una vez que se pide a partir de lo que hay en el Items

    public static decimal Total => Items.Sum(p => p.Precio);

    public static int Cantidad => Items.Count;
}
