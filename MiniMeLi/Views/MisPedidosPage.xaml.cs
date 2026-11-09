using MiniMeLi.ViewModels;

namespace MiniMeLi.Views;

public partial class MisPedidosPage : ContentPage
{
    private readonly MisPedidosViewModel _viewModel = new();

    public MisPedidosPage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarAsync();
    }
}