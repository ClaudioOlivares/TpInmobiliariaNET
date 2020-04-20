using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace clase1posta.Models
{
    public class RepositorioUsuario
    {
        private readonly string connectionString;
        private readonly IConfiguration configuration;

        public RepositorioUsuario(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:DefaultConnection"];

        }

        public int Alta(Usuario p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Usuario (Nombre,Apellido,Email,Clave,Rol) " +
                    $"VALUES (@nombre,@apellido,@email,@clave, @rol);" +
                    $"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@email", p.Email);
                    command.Parameters.AddWithValue("@clave", p.Clave);
                    command.Parameters.AddWithValue("@rol", p.Rol);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.IdUsuario = res;
                    connection.Close();
                }
            }
            return res;
        }
          public Usuario ObtenerPorEmail(String email)
        {
            Usuario u = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdUsuario, Nombre,Apellido,Email,Clave,Rol  FROM Usuario" +
                    $" WHERE Email=@email";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                   
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        u = new Usuario
                        {
                            IdUsuario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Clave = reader.GetString(4),
                            Rol = reader.GetString(5),
                         
                        };
                    }
                    connection.Close();
                }
            }
            return u;
        }



    }
}
