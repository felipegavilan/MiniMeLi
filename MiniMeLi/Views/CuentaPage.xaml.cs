using MiniMeLi.ViewModels;

namespace MiniMeLi.Views;

public partial class CuentaPage : ContentPage
{
    public CuentaPage()
    {
        InitializeComponent();
        BindingContext = new CuentaViewModel();
    }
}