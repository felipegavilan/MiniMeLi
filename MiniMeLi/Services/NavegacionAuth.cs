using MiniMeLi.Views;

namespace MiniMeLi.Services;

public static class NavegacionAuth
{
    // Cierra Login/Registro y vuelve a la página desde la que se abrieron
    public static async Task VolverAlOrigenAsync()
    {
        var nav = Shell.Current.Navigation;
        while (nav.NavigationStack.Count > 1 &&
               nav.NavigationStack[nav.NavigationStack.Count - 1] is LoginPage or RegistroPage)
        {
            await nav.PopAsync(false);
        }
    }
}