using Abstracciones.Modelos;
using Abstracciones.Reglas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class IndexModel : PageModel
    {
        private IConfiguracion _configuracion;

        public IList<ProductoResponse> productos { get; set; } = new List<ProductoResponse>();

        public IndexModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task OnGet()
        {
            // 🔥 evitar cache
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProductos");

            var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(endpoint);

            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();

            // 
            productos = JsonSerializer.Deserialize<List<ProductoResponse>>(resultado,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}