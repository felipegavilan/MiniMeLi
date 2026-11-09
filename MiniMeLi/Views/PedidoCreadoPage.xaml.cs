using MiniMeLi.Models;
using MiniMeLi.ViewModels;

namespace MiniMeLi.Views;

public partial class PedidoCreadoPage : ContentPage
{
    public PedidoCreadoPage(PedidoCreado pedido, List<string> lineas)
    {
        InitializeComponent();
        BindingContext = new PedidoCreadoViewModel(pedido, lineas);
    }
}