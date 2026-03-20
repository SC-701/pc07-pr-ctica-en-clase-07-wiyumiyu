using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flujo
{
    public class ProductoFlujo : IProductoFlujo
    {
        private readonly IProductoDA productoDA;

        public ProductoFlujo(IProductoDA productoDA)
        {
            this.productoDA = productoDA;
        }

        public Task<Guid> Agregar(ProductoRequest producto)
        {
            return productoDA.Agregar(producto);
        }

        public Task<Guid> Editar(Guid Id, ProductoRequest producto)
        {
            return productoDA.Editar(Id,producto);
        }

        public Task<Guid> Eliminar(Guid Id)
        {
            return productoDA.Eliminar(Id);
        }

        public Task<IEnumerable<ProductoResponse>> Obtener()
        {
            return productoDA.Obtener();
        }

        public Task<ProductoDetalle> Obtener(Guid Id)
        {
            return productoDA.Obtener(Id);
        }
    }
}
