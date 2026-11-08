namespace MiniMeLi.Models;

public class AuthResponse
{
    public string Token { get; set; } = "";
    public DateTime Expira { get; set; }
    public string Nombre { get; set; } = "";
    public string Email { get; set; } = "";
    public string Rol { get; set; } = "";
}