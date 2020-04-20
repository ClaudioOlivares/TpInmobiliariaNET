using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace clase1posta.Models
{
    public class RepositorioInmueble
    {
        private readonly string connectionString;
        private readonly IConfiguration configuration;

        public RepositorioInmueble(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:DefaultConnection"];

        }

        public IList<Inmueble> ObtenerTodos()
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT IdInmueble, i.IdPropietario,Direccion,IdTipo,CantAmbientes,Precio,Estado," +
                    " p.Nombre, p.Apellido, t.NombreTipo" +
                    " FROM Inmuebles i INNER JOIN Propietarios p ON i.IdPropietario = p.IdPropietario  " +
                    " INNER JOIN TipoInmueble t ON i.IdTipo = t.IdTipoInmueble";

              /*  string sql = "SELECT IdInmueble, i.IdPropietario,Direccion,IdTipo,CantAmbientes,Precio,Estado" +
                  
                   " FROM Inmuebles i " ;*/
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    try
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Inmueble p = new Inmueble
                            {
                                IdInmueble = reader.GetInt32(0),
                                IdPropietario = reader.GetInt32(1),
                              
                                Direccion = reader.GetString(2),
                                IdTipo = reader.GetInt32(3),
                                CantAmbientes = reader.GetInt32(4),
                                Precio = reader.GetDecimal(5),
                                Estado = reader.GetBoolean(6),
                                Tipo = new TipoInmueble
                                {
                                    idTípoInmueble = reader.GetInt32(3),
                                    nombreTipo = reader.GetString(9),

                                },
                                Propietario = new Propietario
                                {
                                    idPropietario = reader.GetInt32(1),
                                    nombre = reader.GetString(7),
                                    apellido = reader.GetString(8),
                                }
                                

                            };

                            res.Add(p);
                        }

                    }
                    catch(System.Exception ex)
                    { 


                    }
          
                    connection.Close();
                }
            }
            return res;
        }


        public int Alta(Inmueble entidad)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Inmuebles (IdPropietario,Direccion,IdTipo,CantAmbientes,Precio,Estado) " +
                    "VALUES (@IdPropietario, @Direccion, @IdTipo, @CantAmbientes, @Precio, @Estado);" +
                    "SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@IdPropietario", entidad.IdPropietario);
                    command.Parameters.AddWithValue("@Direccion", entidad.Direccion);
                    command.Parameters.AddWithValue("@IdTipo", entidad.IdTipo);
                    command.Parameters.AddWithValue("@CantAmbientes", entidad.CantAmbientes);
                    command.Parameters.AddWithValue("@Precio", entidad.Precio);
                    command.Parameters.AddWithValue("@Estado", entidad.Estado);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    entidad.IdInmueble = res;
                    connection.Close();
                }
            }
            return res;
        }


        public Inmueble ObtenerPorId(int id)
        {
            Inmueble entidad = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT IdInmueble, i.IdPropietario,Direccion,IdTipo,CantAmbientes,Precio,Estado," +
                    " p.Nombre, p.Apellido, t.NombreTipo" +
                    " FROM Inmuebles i INNER JOIN Propietarios p ON i.IdPropietario = p.IdPropietario  " +
                    " INNER JOIN TipoInmueble t ON i.IdTipo = t.IdTipoInmueble " +
                    "  WHERE i.IdInmueble = @id";
              
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    
                    try
                    {
                        var reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            entidad = new Inmueble
                            {
                                IdInmueble = reader.GetInt32(0),
                                IdPropietario = reader.GetInt32(1),
                                Direccion = reader.GetString(2),
                                IdTipo = reader.GetInt32(3),
                                CantAmbientes = reader.GetInt32(4),
                                Precio = reader.GetDecimal(5),
                                Estado = reader.GetBoolean(6),
                                Propietario = new Propietario
                                {
                                    nombre = reader.GetString(7),
                                    apellido = reader.GetString(8),
                                },
                                Tipo = new TipoInmueble
                                {
                                    nombreTipo = reader.GetString(9),
                                }

                            };
                        }
                    }
                    catch(Exception ex)
                    {


                    }
                   
                    connection.Close();
                }
            }
            return entidad;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM Inmuebles WHERE IdInmueble = {id}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }


        public int Modificacion(Inmueble entidad)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Inmuebles SET " +
                    "Direccion=@direccion,IdTipo=@idTipo, CantAmbientes=@cantAmbientes, Precio=@precio, Estado=@estado, IdPropietario = @IdPropietario " +
                    "WHERE IdInmueble = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@direccion", entidad.Direccion);
                    command.Parameters.AddWithValue("@idTipo", entidad.IdTipo);
                    command.Parameters.AddWithValue("@cantAmbientes", entidad.CantAmbientes);
                    command.Parameters.AddWithValue("@precio", entidad.Precio);
                    command.Parameters.AddWithValue("@estado", entidad.Estado);
                    command.Parameters.AddWithValue("@IdPropietario", entidad.IdPropietario);
                    command.Parameters.AddWithValue("@id", entidad.IdInmueble);
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
    }


}

