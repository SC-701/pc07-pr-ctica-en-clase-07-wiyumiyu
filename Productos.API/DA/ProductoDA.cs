using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DA
{
    public class ProductoDA : IProductoDA
    {
        private readonly IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConection;

        public ProductoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<Guid> Agregar(ProductoRequest producto)
        {
            string query = @"Agregar";
            var resultadoConsulta = await _sqlConection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Guid.NewGuid(),
                IdSubCategoria = producto.IdSubCategoria,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                CodigoBarras = producto.CodigoBarras
            });
            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid Id, ProductoRequest producto)
        {
            await verificarProductoExiste(Id);
            string query = @"Editar";
            var resultadoConsulta = await _sqlConection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Id,
                IdSubCategoria = producto.IdSubCategoria,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                CodigoBarras = producto.CodigoBarras
            });
            return resultadoConsulta;
        }

        public async Task<Guid> Eliminar(Guid Id)
        {
            await verificarProductoExiste(Id);
            string query = @"Eliminar";
            var resultadoConsulta = await _sqlConection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Id,
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<ProductoResponse>> Obtener()
        {
            string query = @"Obtener";
            var resultadoConsulta = await _sqlConection.QueryAsync<ProductoResponse>(query);
            return resultadoConsulta;
        }

        public async Task<ProductoDetalle> Obtener(Guid Id)
        {
            string query = @"ObtenerId";
            var resultadoConsulta = await _sqlConection.QueryAsync<ProductoDetalle>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }
        private async Task verificarProductoExiste(Guid Id)
        {
            ProductoResponse? resultadoConsultaProducto = await Obtener(Id);
            if (resultadoConsultaProducto == null)
                throw new Exception("No se encontro el producto");
        }
    }
}
