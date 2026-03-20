using Abstracciones.Modelos;
using Abstracciones.Reglas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class EliminarModel : PageModel
    {
        private IConfiguracion _configuracion;

        [BindProperty]
        public ProductoResponse producto { get; set; } = default!;

        public EliminarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        // 🔹 GET
        public async Task<IActionResult> OnGet(Guid? id)
        {
            if (id == null)
                return NotFound();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProducto");

            var cliente = new HttpClient();

            var res = await cliente.GetAsync(string.Format(endpoint, id));

            res.EnsureSuccessStatusCode();

            if (res.StatusCode == HttpStatusCode.OK)
            {
                var json = await res.Content.ReadAsStringAsync();

                producto = JsonSerializer.Deserialize<ProductoResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return Page();
        }

        // 🔹 POST (DELETE)
        public async Task<IActionResult> OnPost()
        {
            if (producto.Id == Guid.Empty)
                return NotFound();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EliminarProducto");

            var cliente = new HttpClient();

            var res = await cliente.DeleteAsync(string.Format(endpoint, producto.Id));

            res.EnsureSuccessStatusCode();

            return RedirectToPage("./Index");
        }
    }
}