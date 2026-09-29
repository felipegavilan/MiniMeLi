using MiniMeLi.Models;
using System.Collections.ObjectModel;
using MiniMeLi.Services;
using MiniMeLi.Views;


namespace MiniMeLi
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Producto> Productos { get; set; } = new ObservableCollection<Producto>(ProductoService.ObtenerProductosPrueba());

        private async void OnProductoSeleccionado(object sender, TappedEventArgs e)
        {
            //obtenemos el producto que viene en el parametro del toque
            var productoSeleccionado = e.Parameter as Producto;
            if (productoSeleccionado == null) return;
            
            await Navigation.PushAsync(new ProductoDetalle(productoSeleccionado));
        }
        public MainPage()
        {
            InitializeComponent();

            // Inicializar la colección de productos con datos de prueba del servicio
            Productos = new ObservableCollection<Producto>(ProductoService.ObtenerProductosPrueba());

            // Vinculamos el contexto para que la vista encuentre la propiedad Productos
            BindingContext = this;
        }       
    }
}
