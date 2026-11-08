using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiniMeLi.Models;
using MiniMeLi.Services;

namespace MiniMeLi.ViewModels;

public partial class CarritoViewModel : ObservableObject
{
    public ObservableCollection<CartItem> Items => CarritoService.Items;
    public decimal Total => CarritoService.Total;
    public int Cantidad => CarritoService.Cantidad;
    public bool HayItems => Items.Count > 0;

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

    private void Refrescar()
    {
        OnPropertyChanged(nameof(Total));
        OnPropertyChanged(nameof(Cantidad));
        OnPropertyChanged(nameof(HayItems));
    }
}