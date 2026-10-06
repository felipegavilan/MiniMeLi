namespace MiniMeLi.Views;

using MiniMeLi.Models;
using MiniMeLi.Services;

public partial class ProductoDetalle : ContentPage
{
	public ProductoDetalle(Producto producto)
    {
        InitializeComponent();
        BindingContext = producto;
    }

    private async void OnComprarClicked(object? sender, EventArgs e)
    {
        var producto = BindingContext as Producto;
        if (producto != null)
        {
            CarritoService.AgregarProducto(producto);
            await DisplayAlertAsync("Carrito", $"{producto.Titulo} se agregó al carrito", "OK");
        }
    }
}