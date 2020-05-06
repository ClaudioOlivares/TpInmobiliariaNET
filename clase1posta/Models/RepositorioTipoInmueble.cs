using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace clase1posta.Models
{
    public class RepositorioTipoInmueble
    {
        private readonly string connectionString;
        private readonly IConfiguration configuration;

        public RepositorioTipoInmueble(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:DefaultConnection"];

        }

        public IList<TipoInmueble> ObtenerTodos()
        {
            IList<TipoInmueble> res = new List<TipoInmueble>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdTipoInmueble, NombreTipo" +
                    $" FROM TipoInmueble";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        TipoInmueble p = new TipoInmueble
                        {
                            idTípoInmueble = reader.GetInt32(0),
                            nombreTipo = reader.GetString(1),
                           
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }


        public int Alta(TipoInmueble p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO TipoInmueble (NombreTipo) " +
                    $"VALUES (@nombre);" +
                    $"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", p.nombreTipo);


                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.idTípoInmueble = res;
                    connection.Close();
                }
            }
            return res;
        }







    }
}
