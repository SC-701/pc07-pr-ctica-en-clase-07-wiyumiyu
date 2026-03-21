using Abstracciones.Modelos;
using Abstracciones.Reglas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

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
        public List<SelectListItem> subCategorias { get; set; } = new();

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
            if (id == null)
                return NotFound();
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProducto");
            var cliente = new HttpClient();

            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id));
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                await ObtenerCategoriasAsync();
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                producto = JsonSerializer.Deserialize<ProductoResponse>(resultado, opciones);

                if (producto != null)
                {
                    categoriaSeleccionada = Guid.Parse(categorias.Where(m => m.Text == producto.Categoria).FirstOrDefault().Value);
                    subCategorias = (await ObtenerSubCategoriasAsync(categoriaSeleccionada)).Select(a =>
                        new SelectListItem
                        {
                            Value = a.Id.ToString(),
                            Text = a.Nombre.ToString(),
                            Selected = a.Nombre == producto.SubCategoria
                        }).ToList();
                    subCategoriaSeleccionada = Guid.Parse(subCategorias.Where(m => m.Text == producto.SubCategoria).FirstOrDefault().Value);
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

            // 🔥 OBTENER VALOR REAL DEL FORM
            var subCategoriaForm = Request.Form["subCategoriaSeleccionada"];

            if (string.IsNullOrEmpty(subCategoriaForm))
            {
                ModelState.AddModelError("", "Seleccione una subcategoría");
                await ObtenerCategoriasAsync();
                return Page();
            }

            Guid subCategoriaId = Guid.Parse(subCategoriaForm);

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EditarProducto");

            var cliente = new HttpClient();

            var respuesta = await cliente.PutAsJsonAsync(
                string.Format(endpoint, producto.Id),
                new ProductoRequest
                {
                    IdSubCategoria = subCategoriaId, //
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    CodigoBarras = producto.CodigoBarras
                });

            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Index");
        }


        // categorias
        private async Task ObtenerCategoriasAsync()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerCategorias");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var resultadoDeserializado = JsonSerializer.Deserialize<List<Categoria>>(resultado, opciones);
                categorias = resultadoDeserializado.Select(a =>
                                  new SelectListItem
                                  {
                                      Value = a.Id.ToString(),
                                      Text = a.Nombre.ToString()
                                  }).ToList();
            }
        }

        //  subcategorias
        public async Task<JsonResult> OnGetObtenerSubCategorias(Guid idCategoria)
        {
            var subCategorias = await ObtenerSubCategoriasAsync(idCategoria);
            return new JsonResult(subCategorias);
        }

        private async Task<List<SubCategoria>> ObtenerSubCategoriasAsync(Guid idCategoria)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerSubCategorias");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, idCategoria));

            var respuesta = await cliente.SendAsync(solicitud);
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