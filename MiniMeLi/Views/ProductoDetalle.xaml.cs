namespace MiniMeLi.Views;

using MiniMeLi.Models;

public partial class ProductoDetalle : ContentPage
{
	public ProductoDetalle(Producto producto)
    {
        InitializeComponent();
        BindingContext = producto;
    }

    private async void OnAlertButtonClicked(object? sender, EventArgs e)
    {
        var producto = BindingContext as Producto;
        if (producto != null)
        {
            await DisplayAlertAsync("Agregado a tu Carrito de Compras", $"Tocaste: {producto.Titulo} - ${producto.Precio}", "OK");
        }
    }

}