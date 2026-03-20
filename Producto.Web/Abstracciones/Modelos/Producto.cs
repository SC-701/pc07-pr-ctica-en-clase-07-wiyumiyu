using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    // BASE → Validaciones para Web (Model Binding)
    public class ProductoBase
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 999999999, ErrorMessage = "El precio debe ser mayor que 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El código de barras es obligatorio.")]
        [StringLength(50, ErrorMessage = "El código de barras no puede superar los 50 caracteres.")]
        public string CodigoBarras { get; set; }
    }

    // REQUEST → Lo que entra desde API
    public class ProductoRequest : ProductoBase
    {
        [Required(ErrorMessage = "La subcategoría es obligatoria.")]
        public Guid IdSubCategoria { get; set; }
    }

    // RESPONSE → Lo que sale hacia el cliente
    public class ProductoResponse : ProductoBase
    {
        public Guid Id { get; set; }
        public string? SubCategoria { get; set; }
        public string? Categoria { get; set; }
    }

    // DETALLE → Información extendida
    public class ProductoDetalle : ProductoResponse
    {
        public decimal PrecioUSD { get; set; }
    }
}