using Abstracciones.Modelos;
using Abstracciones.Reglas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class EditarModel : PageModel
    {
        private IConfiguracion _configuracion;

        [BindProperty]
        public ProductoResponse producto { get; set; } = default!;

        [BindProperty]
        public List<SelectListItem> categorias { get; set; } = new();

        [BindProperty]
        public List<SelectListItem> subcategorias { get; set; } = new();

        [BindProperty]
        public Guid categoriaSeleccionada { get; set; }

        [BindProperty]
        public Guid subCategoriaSeleccionada { get; set; }

        public EditarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        // GET
        public async Task<IActionResult> OnGet(Guid id)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProducto");

            var cliente = new HttpClient();
            var res = await cliente.GetAsync(string.Format(endpoint, id));

            var json = await res.Content.ReadAsStringAsync();

            producto = JsonSerializer.Deserialize<ProductoResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await ObtenerCategoriasAsync();

            if (producto != null)
            {
                var categoria = categorias
                    .FirstOrDefault(c => c.Text.Contains(producto.Categoria ?? "", StringComparison.OrdinalIgnoreCase));

                if (categoria != null)
                {
                    categoriaSeleccionada = Guid.Parse(categoria.Value);

                    var lista = await ObtenerSubCategoriasAsync(categoriaSeleccionada);

                    subcategorias = lista.Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Nombre
                    }).ToList();

                    var sub = subcategorias
    .FirstOrDefault(s => s.Text.Contains(producto.SubCategoria ?? "", StringComparison.OrdinalIgnoreCase));

                    if (sub != null)
                    {
                        subCategoriaSeleccionada = Guid.Parse(sub.Value);
                    }
                }
            }

            return Page();
        }

        // POST
        public async Task<IActionResult> OnPost()
        {
            if (producto.Id == Guid.Empty)
                return NotFound();

            if (!ModelState.IsValid)
                return Page();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EditarProducto");

            var cliente = new HttpClient();

            var response = await cliente.PutAsJsonAsync(
                string.Format(endpoint, producto.Id),
                new ProductoRequest
                {
                    IdSubCategoria = subCategoriaSeleccionada, // 🔥 IGUAL QUE VEHICULO
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    CodigoBarras = producto.CodigoBarras
                });

            response.EnsureSuccessStatusCode();
            Console.WriteLine(producto.Id);
            Console.WriteLine(subCategoriaSeleccionada);
            Console.WriteLine(producto.Nombre);

            return RedirectToPage("./Index");
        }

        // categorias
        private async Task ObtenerCategoriasAsync()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerCategorias");

            var cliente = new HttpClient();
            var res = await cliente.GetAsync(endpoint);

            var json = await res.Content.ReadAsStringAsync();

            var lista = JsonSerializer.Deserialize<List<Categoria>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            categorias = lista.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            }).ToList();
        }

        //  subcategorias
        public async Task<JsonResult> OnGetObtenerSubCategorias(Guid idCategoria)
        {
            return new JsonResult(await ObtenerSubCategoriasAsync(idCategoria));
        }

        private async Task<List<SubCategoria>> ObtenerSubCategoriasAsync(Guid idCategoria)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerSubCategorias");

            var cliente = new HttpClient();

            var res = await cliente.GetAsync(string.Format(endpoint, idCategoria));

            var json = await res.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<SubCategoria>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}