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
        if (BindingContext is not Producto producto) return;

        if (producto.SinStock)
        {
            await DisplayAlertAsync("Sin stock", "Este producto no está disponible por el momento.", "OK");
            return;
        }

        // No se puede pedir más de lo que hay
        if (CarritoService.CantidadDe(producto.IdProd) >= producto.Stock)
        {
            await DisplayAlertAsync("Stock máximo",
                $"Ya tenés en el carrito todas las unidades disponibles ({producto.Stock}).", "OK");
            return;
        }

        CarritoService.AgregarProducto(producto);
        await DisplayAlertAsync("Carrito", $"{producto.Descripcion} se agregó al carrito", "OK");
    }
}