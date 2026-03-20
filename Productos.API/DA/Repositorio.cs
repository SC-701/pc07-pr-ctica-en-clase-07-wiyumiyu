using Abstracciones.Interfaces.DA;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class Repositorio : IRepositorioDapper
    {
        private readonly IConfiguration _configuracion;
        private readonly SqlConnection _connection;

        public Repositorio(IConfiguration configuracion)
        {
            _configuracion = configuracion;
            _connection = new SqlConnection(
                _configuracion.GetConnectionString("BD")
            );
        }
        public SqlConnection ObtenerRepositorio()
        {
            return _connection;
        }
    }
}
