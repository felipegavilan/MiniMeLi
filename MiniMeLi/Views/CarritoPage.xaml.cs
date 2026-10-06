using MiniMeLi.ViewModels;

namespace MiniMeLi.Views;

public partial class CarritoPage : ContentPage
{
	public CarritoPage()
	{
		InitializeComponent();
		BindingContext = new CarritoViewModel();
	}
}