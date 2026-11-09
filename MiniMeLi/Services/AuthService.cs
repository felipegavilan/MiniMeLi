using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using MiniMeLi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiniMeLi.Services;

public static class AuthService
{
    private const string KToken = "auth_token";
    private const string KExpira = "auth_expira";
    private const string KNombre = "auth_nombre";
    private const string KEmail = "auth_email";
    private const string KRol = "auth_rol";

    // Cliente para llamadas que necesitan sesión (lleva el token en el header)
    public static HttpClient Http { get; } = new() { Timeout = TimeSpan.FromSeconds(30) };

    private static bool _inicializado;

    public static string Token { get; private set; } = "";
    public static string Nombre { get; private set; } = "";
    public static string Email { get; private set; } = "";
    public static string Rol { get; private set; } = "";
    public static DateTime Expira { get; private set; }

    public static bool EstaLogueado => !string.IsNullOrEmpty(Token) && Expira > DateTime.UtcNow;

    // Recupera la sesión guardada. Solo hace algo la primera vez.
    public static async Task InicializarAsync()
    {
        if (_inicializado) return;
        _inicializado = true;
        try
        {
            var token = await SecureStorage.Default.GetAsync(KToken);
            var expiraTxt = await SecureStorage.Default.GetAsync(KExpira);
            if (string.IsNullOrEmpty(token) ||
                !DateTime.TryParse(expiraTxt, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var expira) ||
                expira.ToUniversalTime() <= DateTime.UtcNow)
            {
                CerrarSesion(); // no hay sesión o ya venció
                return;
            }

            Token = token;
            Expira = expira.ToUniversalTime();
            Nombre = await SecureStorage.Default.GetAsync(KNombre) ?? "";
            Email = await SecureStorage.Default.GetAsync(KEmail) ?? "";
            Rol = await SecureStorage.Default.GetAsync(KRol) ?? "";
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }
        catch (Exception ex)
        {
            // Si el almacenamiento seguro falla, arrancamos sin sesión
            System.Diagnostics.Debug.WriteLine($"Error al leer la sesión: {ex.Message}");
            CerrarSesion();
        }
    }

    public static Task<(bool Ok, string Mensaje)> LoginAsync(string email, string password) =>
        EnviarAsync("auth/login", new { Email = email.Trim(), Password = password });

    public static Task<(bool Ok, string Mensaje)> RegistroAsync(string nombre, string email, string? telefono, string password) =>
        EnviarAsync("auth/registro", new
        {
            Nombre = nombre.Trim(),
            Email = email.Trim(),
            Telefono = string.IsNullOrWhiteSpace(telefono) ? null : telefono.Trim(),
            Password = password
        });

    public static void CerrarSesion()
    {
        Token = Nombre = Email = Rol = "";
        Expira = default;
        Http.DefaultRequestHeaders.Authorization = null;
        try
        {
            SecureStorage.Default.Remove(KToken);
            SecureStorage.Default.Remove(KExpira);
            SecureStorage.Default.Remove(KNombre);
            SecureStorage.Default.Remove(KEmail);
            SecureStorage.Default.Remove(KRol);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al borrar la sesión: {ex.Message}");
        }
    }

    private static async Task<(bool Ok, string Mensaje)> EnviarAsync(string ruta, object cuerpo)
    {
        try
        {
            var contenido = new StringContent(JsonConvert.SerializeObject(cuerpo), Encoding.UTF8, "application/json");
            var respuesta = await Http.PostAsync(AppConfig.ApiBaseUrl + ruta, contenido);
            var json = await respuesta.Content.ReadAsStringAsync();

            if (!respuesta.IsSuccessStatusCode)
                return (false, LeerMensaje(json) ?? $"Error del servidor ({(int)respuesta.StatusCode}).");

            var sesion = JsonConvert.DeserializeObject<AuthResponse>(json);
            if (sesion == null || string.IsNullOrEmpty(sesion.Token))
                return (false, "Respuesta inválida del servidor.");

            await GuardarSesionAsync(sesion);
            return (true, "");
        }
        catch (TaskCanceledException)
        {
            return (false, "El servidor tardó demasiado en responder. Intentá de nuevo.");
        }
        catch (HttpRequestException)
        {
            return (false, "No pudimos conectar con el servidor. Revisá tu conexión.");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error de autenticación: {ex.Message}");
            return (false, "Ocurrió un error inesperado. Intentá de nuevo.");
        }
    }

    private static async Task GuardarSesionAsync(AuthResponse s)
    {
        Token = s.Token;
        Nombre = s.Nombre;
        Email = s.Email;
        Rol = s.Rol;
        Expira = s.Expira.ToUniversalTime();
        Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        _inicializado = true;

        try
        {
            await SecureStorage.Default.SetAsync(KToken, s.Token);
            await SecureStorage.Default.SetAsync(KExpira, Expira.ToString("o", CultureInfo.InvariantCulture));
            await SecureStorage.Default.SetAsync(KNombre, s.Nombre);
            await SecureStorage.Default.SetAsync(KEmail, s.Email);
            await SecureStorage.Default.SetAsync(KRol, s.Rol);
        }
        catch (Exception ex)
        {
            // La sesión sigue activa en memoria, pero no se recordará al cerrar la app
            System.Diagnostics.Debug.WriteLine($"No se pudo guardar la sesión: {ex.Message}");
        }
    }

    public static string? LeerMensaje(string json)
    {
        try
        {
            var obj = JObject.Parse(json);
            return obj["mensaje"]?.ToString() ?? obj["title"]?.ToString();
        }
        catch
        {
            return null;
        }
    }
}