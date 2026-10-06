using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MiniMeLi.Models;
using MiniMeLi.Services;

namespace MiniMeLi.ViewModels;

public partial class CarritoViewModel : ObservableObject
{
    public ObservableCollection<Producto> Items => CarritoService.Items;
    public decimal Total => CarritoService.Total;
    public int Cantidad => CarritoService.Cantidad;

    public CarritoViewModel()
    {
        // Cada vez que se agregue o elimine un producto del carrito, se notificará a la vista para actualizar los valores de Total y Cantidad
        Items.CollectionChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(Cantidad));
        };
    }
}
