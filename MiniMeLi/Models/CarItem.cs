using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace MiniMeLi.Models;

public partial class CartItem : ObservableObject
{
    public Producto Producto { get; set; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Subtotal))]
    private int _cantidad = 1;

    [JsonIgnore]
    public decimal Subtotal => Producto.Precio * Cantidad;
}