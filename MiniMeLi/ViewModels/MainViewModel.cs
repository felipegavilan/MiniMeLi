using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using MiniMeLi.Models;
using MiniMeLi.Services;

namespace MiniMeLi.ViewModels;

public partial class MainViewModel : ObservableObject
{

    //Lista 1: todos los productos.
    private List<Producto> _todosLosProductos = new();

    //Lista 2: lo que se muestra en pantalla, que puede ser filtrado.
    
    public ObservableCollection<Producto> Productos { get; } = new();

    //El toolkit genera una propiedad pública de solo lectura para la lista de productos, que se puede enlazar a la vista. "SearchText"
    [ObservableProperty] 
    private string _searchText = string.Empty;

    //El toolkit genera la declaración de este método parcial.
    //Le damos el cuerpo y se ejecutará cada vez que cambie el valor de la propiedad SearchText.

    partial void OnSearchTextChanged(string value)
    {
        AplicarFiltro();
    }

    //El guard de OnAppearing mira la lista completa, no la filtrada.
    public bool HayProductosCargados => _todosLosProductos.Count > 0;

    public async Task CargarProductosAsync()
    {
        _todosLosProductos = await ProductoService.ObtenerProductosAsync();
        AplicarFiltro();

    }
    

    private void AplicarFiltro()
    {
        var texto = _searchText?.Trim();

        //si no hay texto, se muestra la lista completa. Si hay, se filtra por titulo.
        IEnumerable<Producto> resultado = string.IsNullOrEmpty(texto)
            ? _todosLosProductos
            : _todosLosProductos.Where(p => p.Titulo?.Contains(texto, StringComparison.OrdinalIgnoreCase) == true);

        //Actualizamos la lista de productos que se muestra en pantalla.
        Productos.Clear();
        foreach (var p in resultado)
        {
            Productos.Add(p);
        }
    }
}
