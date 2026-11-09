using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntelliJ.Lang.Annotations;
using MiniMeLi.Models;
using MiniMeLi.Services;
using MiniMeLi.Views;
using System.Collections.ObjectModel;

namespace MiniMeLi.ViewModels;

public partial class CarritoViewModel : ObservableObject
{
    public ObservableCollection<CartItem> Items => CarritoService.Items;
    public decimal Total => CarritoService.Total;
    public int Cantidad => CarritoService.Cantidad;
    public bool HayItems => Items.Count > 0;

    [ObservableProperty]
    private bool _isBusy;

    public CarritoViewModel()
    {
        CarritoService.Cargar();
    }

    [RelayCommand]
    private void Aumentar(CartItem item)
    {
        CarritoService.Aumentar(item);
        Refrescar();
    }

    [RelayCommand]
    private void Disminuir(CartItem item)
    {
        CarritoService.Disminuir(item);
        Refrescar();
    }

    [RelayCommand]
    private void Quitar(CartItem item)
    {
        CarritoService.Quitar(item);
        Refrescar();
    }

    [RelayCommand]
    private async Task Vaciar()
    {
        bool ok = await Shell.Current.DisplayAlertAsync("Vaciar carrito", "¿Querés quitar todos los productos?", "Sí", "No");
        if (!ok) return;
        CarritoService.Vaciar();
        Refrescar();
    }

    [RelayCommand]
    private async Task FinalizarCompra()
    {
        if (Items.Count == 0) return;

        // 1. Hace falta sesión
        await AuthService.InicializarAsync();
        if (!AuthService.EstaLogueado)
        {
            bool ingresar = await Shell.Current.DisplayAlertAsync("Iniciá sesión",
                "Para finalizar tu compra necesitás ingresar o crear una cuenta. Tu carrito se mantiene.",
                "Ingresar", "Ahora no");
            if (ingresar)
                await Shell.Current.Navigation.PushAsync(new LoginPage());
            return;
        }

        // 2. Confirmación
        bool confirmar = await Shell.Current.DisplayAlertAsync("Confirmar pedido",
            $"Vas a generar un pedido por {Total:C}. ¿Continuás?", "Sí, continuar", "Cancelar");
        if (!confirmar) return;

        // Guardamos el detalle antes de vaciar el carrito (lo usa el mensaje de WhatsApp)
        var lineas = Items.Select(i => $"{i.Cantidad} x {i.Producto.Descripcion}").ToList();

        // 3. Crear el pedido en la API
        IsBusy = true;
        var resultado = await PedidoService.CrearPedidoAsync(Items);
        IsBusy = false;

        if (!resultado.Ok || resultado.Pedido == null)
        {
            await Shell.Current.DisplayAlertAsync("No pudimos crear el pedido", resultado.Mensaje, "OK");
            return;
        }

        // 4. Éxito: se vacía el carrito y se muestra el pedido
        CarritoService.Vaciar();
        Refrescar();
        await Shell.Current.Navigation.PushAsync(new PedidoCreadoPage(resultado.Pedido, lineas));
    }

    private void Refrescar()
    {
        OnPropertyChanged(nameof(Total));
        OnPropertyChanged(nameof(Cantidad));
        OnPropertyChanged(nameof(HayItems));
    }
}