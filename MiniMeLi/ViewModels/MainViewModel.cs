using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using MiniMeLi.Models;
using MiniMeLi.Services;

namespace MiniMeLi.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public ObservableCollection<Producto> Productos { get; } = new();

    public async Task CargarProductosAsync()
    {
        var lista = await ProductoService.ObtenerProductosAsync();

        Productos.Clear();
        foreach (var p in lista)
        {
            Productos.Add(p);
        }
    }
}
