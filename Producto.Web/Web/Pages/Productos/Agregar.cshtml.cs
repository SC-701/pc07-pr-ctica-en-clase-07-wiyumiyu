using Abstracciones.Modelos;
using Abstracciones.Reglas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class AgregarModel : PageModel
    {
        private IConfiguracion _configuracion;

        [BindProperty]
        public ProductoRequest producto { get; set; } = default!;

        [BindProperty]
        public List<SelectListItem> categorias { get; set; } = new();

        [BindProperty]
        public List<SelectListItem> subcategorias { get; set; } = new();

        [BindProperty]
        public Guid categoriaSeleccionada { get; set; }

      
        [BindProperty]
        public Guid subCategoriaSeleccionada { get; set; }

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        // GET
        public async Task<IActionResult> OnGet()
        {
            await ObtenerCategoriasAsync();
            return Page();
        }

        // POST
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                await ObtenerCategoriasAsync();
                return Page();
            }

            // 🔥 CLAVE: asignar subcategoria
            producto.IdSubCategoria = subCategoriaSeleccionada;

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "AgregarProducto");

            var cliente = new HttpClient();

            var respuesta = await cliente.PostAsJsonAsync(endpoint, producto);

            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Index");
        }

        // CATEGORIAS
        private async Task ObtenerCategoriasAsync()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerCategorias");

            var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(endpoint);

            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();

                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                var lista = JsonSerializer.Deserialize<List<Categoria>>(resultado, opciones);

                categorias = lista.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre
                }).ToList();
            }
        }

        // AJAX handler
        public async Task<JsonResult> OnGetObtenerSubCategorias(Guid idCategoria)
        {
            var lista = await ObtenerSubCategoriasAsync(idCategoria);
            return new JsonResult(lista);
        }

        // SUBCATEGORIAS
        private async Task<List<SubCategoria>> ObtenerSubCategoriasAsync(Guid idCategoria)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerSubCategorias");

            var cliente = new HttpClient();

            var respuesta = await cliente.GetAsync(string.Format(endpoint, idCategoria));

            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();

                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                return JsonSerializer.Deserialize<List<SubCategoria>>(resultado, opciones);
            }

            return new List<SubCategoria>();
        }
    }
}