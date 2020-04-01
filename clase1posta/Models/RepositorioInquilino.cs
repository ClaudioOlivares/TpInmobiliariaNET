using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace clase1posta.Models
{
    public class RepositorioInquilino
    {

        private readonly string connectionString;
        private readonly IConfiguration configuration;

        public RepositorioInquilino(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:DefaultConnection"];

        }

        public IList<Inquilino> ObtenerTodos()
        {
            IList<Inquilino> res = new List<Inquilino>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInquilino, Nombre,Apellido,Dni,Trabajo,NombreGarante,ApellidoGarante,DniGarante" +
                    $" FROM Inquilinos";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inquilino p = new Inquilino
                        {
                            idInquilino = reader.GetInt32(0),
                            nombre = reader.GetString(1),
                            apellido = reader.GetString(2),
                            dni = reader.GetString(3),
                            trabajo = reader.GetString(4),
                            nombreGarante = reader.GetString(5),
                            apellidoGarante = reader.GetString(6),
                            dniGarante = reader.GetString(7),

                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }


        public int Alta(Inquilino p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Inquilinos (Nombre,Apellido,Dni,Trabajo,NombreGarante,ApellidoGarante,DniGarante) " +
                    $"VALUES (@nombre,@apellido,@dni,@trabajo,@nombreGarante, @apellidoGarante,@dniGarante);" +
                    $"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", p.nombre);
                    command.Parameters.AddWithValue("@apellido", p.apellido);
                    command.Parameters.AddWithValue("@dni", p.dni);
                    command.Parameters.AddWithValue("@trabajo", p.trabajo);
                    command.Parameters.AddWithValue("@nombreGarante", p.nombreGarante);
                    command.Parameters.AddWithValue("@apellidoGarante", p.apellidoGarante);
                    command.Parameters.AddWithValue("@dniGarante", p.dniGarante);



                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.idInquilino = res;
                    connection.Close();
                }
            }
            return res;
        }


        public int Baja(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM Inquilinos WHERE IdInquilino = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }


        public Inquilino ObtenerPorId(int id)
        {
            Inquilino p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInquilino, Nombre,Apellido,Dni,Trabajo,NombreGarante,ApellidoGarante,DniGarante FROM Inquilinos" +
                    $" WHERE IdInquilino=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Inquilino
                        {
                            idInquilino = reader.GetInt32(0),
                            nombre = reader.GetString(1),
                            apellido = reader.GetString(2),
                            dni = reader.GetString(3),
                            trabajo = reader.GetString(4),
                            nombreGarante = reader.GetString(5),
                            apellidoGarante = reader.GetString(6),
                            dniGarante = reader.GetString(7),

                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }


        public int Modificacion(Inquilino p)
        {
            int j = 0;
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Inquilinos SET Nombre=@nombre,Apellido=@apellido,Dni=@dni,Trabajo=@trabajo,NombreGarante=@nombreGarante,ApellidoGarante=@apellidoGarante,DniGarante=@dniGarante " +
                    $"WHERE IdInquilino = @idInquilino";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", p.nombre);
                    command.Parameters.AddWithValue("@apellido", p.apellido);
                    command.Parameters.AddWithValue("@dni", p.dni);
                    command.Parameters.AddWithValue("@trabajo", p.trabajo);
                    command.Parameters.AddWithValue("@nombreGarante", p.nombreGarante);
                    command.Parameters.AddWithValue("@apellidoGarante", p.apellidoGarante);
                    command.Parameters.AddWithValue("@dniGarante", p.dniGarante);
                    command.Parameters.AddWithValue("@idInquilino", p.idInquilino);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                   
                    connection.Close();
                }
            }
            return res;
        }
    }
}
