using MiniMeLi.ViewModels;

namespace MiniMeLi.Views;

public partial class RegistroPage : ContentPage
{
    public RegistroPage()
    {
        InitializeComponent();
        BindingContext = new RegistroViewModel();
    }
}