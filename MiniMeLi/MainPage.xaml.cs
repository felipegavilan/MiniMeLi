using MiniMeLi.Models;
using System.Collections.ObjectModel;
using MiniMeLi.Services;


namespace MiniMeLi
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Producto> Productos { get; set; } = new ObservableCollection<Producto>(ProductoService.ObtenerProductosPrueba());

        private Producto? _productoSeleccionado;
        public Producto? ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set
            {
                if (_productoSeleccionado != value)
                {
                    _productoSeleccionado = value;
                    OnPropertyChanged();

                    if (_productoSeleccionado != null)
                    {
                        MostrarDetalleProducto(_productoSeleccionado);

                    }
                }
            }
        }

        public MainPage()
        {
            InitializeComponent();

            // Inicializar la colección de productos con datos de prueba del servicio
            Productos = new ObservableCollection<Producto>(ProductoService.ObtenerProductosPrueba());

            // Vinculamos el contexto para que la vista encuentre la propiedad Productos
            BindingContext = this;
        }

        private async void MostrarDetalleProducto(Producto producto)
        {
            await DisplayAlertAsync("Producto Seleccionado", $"Tocaste: {producto.Titulo} - ${producto.Precio}", "OK");
        }
       
    }
}
