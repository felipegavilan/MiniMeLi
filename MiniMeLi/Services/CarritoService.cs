using System.Collections.ObjectModel;
using MiniMeLi.Models;
using Newtonsoft.Json;

namespace MiniMeLi.Services;

public static class CarritoService
{
    private static readonly string _archivo = Path.Combine(FileSystem.AppDataDirectory, "carrito.json");
    private static bool _cargado;

    public static ObservableCollection<CartItem> Items { get; } = new();

    public static decimal Total => Items.Sum(i => i.Subtotal);
    public static int Cantidad => Items.Sum(i => i.Cantidad);

    // Lee el carrito guardado. Solo lo hace la primera vez que se usa.
    public static void Cargar()
    {
        if (_cargado) return;
        _cargado = true;
        try
        {
            if (!File.Exists(_archivo)) return;
            var json = File.ReadAllText(_archivo);
            var guardados = JsonConvert.DeserializeObject<List<CartItem>>(json);
            if (guardados == null) return;
            foreach (var item in guardados) Items.Add(item);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al leer el carrito: {ex.Message}");
        }
    }

    public static void AgregarProducto(Producto producto)
    {
        Cargar();
        var existente = Items.FirstOrDefault(i => i.Producto.IdProd == producto.IdProd);
        if (existente != null)
            existente.Cantidad++;
        else
            Items.Add(new CartItem { Producto = producto });
        Guardar();
    }

    public static void Aumentar(CartItem item)
    {
        item.Cantidad++;
        Guardar();
    }

    public static void Disminuir(CartItem item)
    {
        if (item.Cantidad > 1) item.Cantidad--;
        else Items.Remove(item);
        Guardar();
    }

    public static void Quitar(CartItem item)
    {
        Items.Remove(item);
        Guardar();
    }

    public static void Vaciar()
    {
        Items.Clear();
        Guardar();
    }

    private static void Guardar()
    {
        try
        {
            File.WriteAllText(_archivo, JsonConvert.SerializeObject(Items));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al guardar el carrito: {ex.Message}");
        }
    }
}