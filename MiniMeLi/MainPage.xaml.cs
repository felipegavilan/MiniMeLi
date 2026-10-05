using MiniMeLi.Models;
using MiniMeLi.Views;
using MiniMeLi.ViewModels;


namespace MiniMeLi
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel = new();
        
        public MainPage()
        {
            InitializeComponent();
            // Vinculamos el contexto para que la vista encuentre la propiedad Productos
            BindingContext = _viewModel;
        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel.Productos.Count > 0) return;
            await _viewModel.CargarProductosAsync();
        }

        private async void OnProductoSeleccionado(object sender, TappedEventArgs e)
        {
            //obtenemos el producto que viene en el parametro del toque
            var productoSeleccionado = e.Parameter as Producto;
            if (productoSeleccionado == null) return;

            await Navigation.PushAsync(new ProductoDetalle(productoSeleccionado));
        }
    }
}
