using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace clase1posta.Models
{

    public class RepositorioContrato
    {
        private readonly string connectionString;
        private readonly IConfiguration configuration;

        public RepositorioContrato(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:DefaultConnection"];

        }

        public IList<Contrato> ObtenerTodos()
        {
            IList<Contrato> res = new List<Contrato>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdContrato,c.IdInquilino,c.IdInmueble,Duracion,FechaInicio,FechaFinal,c.Precio," +
                     " i.Dni, t.Direccion, t.Precio" +
                    " FROM Contratos c INNER JOIN Inquilinos i ON i.IdInquilino = c.IdInquilino  " +
                    " INNER JOIN Inmuebles t ON c.IdInmueble = t.IdInmueble";
              
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {


                        Contrato c = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            IdInquilino = reader.GetInt32(1),
                            IdInmueble = reader.GetInt32(2),
                            Duracion = reader.GetInt32(3),
                            FechaInicio = reader.GetDateTime(4),
                            FechaFinal = reader.GetDateTime(5),
                            Precio = reader.GetDecimal(6),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(8),
                                Precio = reader.GetDecimal(9),                               
                            },
                            Inquilino = new Inquilino
                            {
                                dni = reader.GetString(7),
                            }


                        };
                        res.Add(c);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Contrato> ObtenerTodosPorId(int id)
        {
            IList<Contrato> res = new List<Contrato>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdContrato,c.IdInquilino,c.IdInmueble,Duracion,FechaInicio,FechaFinal,c.Precio, " +
                     " i.Dni, t.Direccion, t.Precio" +
                    " FROM Contratos c INNER JOIN Inquilinos i ON i.IdInquilino = c.IdInquilino  " +
                    " INNER JOIN Inmuebles t ON c.IdInmueble = t.IdInmueble AND c.IdInmueble = @id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {


                        Contrato c = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            IdInquilino = reader.GetInt32(1),
                            IdInmueble = reader.GetInt32(2),
                            Duracion = reader.GetInt32(3),
                            FechaInicio = reader.GetDateTime(4),
                            FechaFinal = reader.GetDateTime(5),
                            Precio = reader.GetDecimal(6),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(8),
                                Precio = reader.GetDecimal(9),
                            },
                            Inquilino = new Inquilino
                            {
                                dni = reader.GetString(7),
                            }


                        };
                        res.Add(c);
                    }
                    connection.Close();
                }
            }
            return res;
        }






        public int Alta(Contrato c)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Contratos (IdInquilino,IdInmueble,Duracion,FechaInicio,FechaFinal,Precio) " +
                    $"VALUES (@idInquilino,@idInmueble,@duracion, @fechaInicio, @fechaFinal,@precio);" +
                    $"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@idInquilino", c.IdInquilino);                
                    command.Parameters.AddWithValue("@idInmueble", c.IdInmueble);
                    command.Parameters.AddWithValue("@duracion", c.Duracion);
                    command.Parameters.AddWithValue("@fechaInicio", c.FechaInicio);
                    command.Parameters.AddWithValue("@fechaFinal", c.FechaFinal);
                    command.Parameters.AddWithValue("@precio", c.Precio);


                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    c.IdContrato = res;
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
                string sql = $"DELETE FROM Contratos WHERE IdContrato = {id}";
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

        public Contrato ObtenerPorId(int id)
        {
            Contrato res = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdContrato,c.IdInquilino,c.IdInmueble,Duracion,FechaInicio,FechaFinal,c.Precio," +
                     " i.Dni, t.Direccion, t.Precio" +
                    " FROM Contratos c INNER JOIN Inquilinos i ON i.IdInquilino = c.IdInquilino  " +
                    " INNER JOIN Inmuebles t ON c.IdInmueble = t.IdInmueble" +
                    " WHERE c.IdContrato = @id ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {


                       res = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            IdInquilino = reader.GetInt32(1),
                            IdInmueble = reader.GetInt32(2),
                            Duracion = reader.GetInt32(3),
                            FechaInicio = reader.GetDateTime(4),
                            FechaFinal = reader.GetDateTime(5),
                           Precio = reader.GetDecimal(6),
                           Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(8),
                                Precio = reader.GetDecimal(9),
                            },
                            Inquilino = new Inquilino
                            {
                                dni = reader.GetString(7),
                            }


                        };
                        
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Contrato entidad)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Contratos SET " +
                    "IdInquilino = @idInquilino,IdInmueble = @idInmueble, Duracion = @duracion,FechaInicio = @fechaInicio ,FechaFinal = @fechaFinal, Precio = @precio " +
                    "WHERE IdContrato = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idInquilino", entidad.IdInquilino);
                    command.Parameters.AddWithValue("@idInmueble", entidad.IdInmueble);
                    command.Parameters.AddWithValue("@duracion", entidad.Duracion);
                    command.Parameters.AddWithValue("@fechaInicio", entidad.FechaInicio);
                    command.Parameters.AddWithValue("@fechaFinal", entidad.FechaFinal);
                    command.Parameters.AddWithValue("@precio", entidad.Precio);
                    command.Parameters.AddWithValue("@id", entidad.IdContrato);
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }


        public Contrato TraerFechaCercana(DateTime fecha, DateTime fechaCierre, int id)
        {
            Contrato res = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdContrato,c.IdInquilino,c.IdInmueble,Duracion,FechaInicio,FechaFinal,c.Precio," +
                     " i.Dni, t.Direccion, t.Precio" +
                    " FROM Contratos c INNER JOIN Inquilinos i ON i.IdInquilino = c.IdInquilino  " +
                    " INNER JOIN Inmuebles t ON c.IdInmueble = t.IdInmueble" +
                    " WHERE c.FechaFinal > @fecha AND @fechaCierre > FechaInicio And c.IdInmueble = @id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.Parameters.Add("@fecha", SqlDbType.Date).Value = fecha;
                    command.Parameters.Add("@fechaCierre", SqlDbType.Date).Value = fechaCierre;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {


                        res = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            IdInquilino = reader.GetInt32(1),
                            IdInmueble = reader.GetInt32(2),
                            Duracion = reader.GetInt32(3),
                            FechaInicio = reader.GetDateTime(4),
                            FechaFinal = reader.GetDateTime(5),
                            Precio = reader.GetDecimal(6),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(8),
                                Precio = reader.GetDecimal(9),
                            },
                            Inquilino = new Inquilino
                            {
                                dni = reader.GetString(7),
                            }


                        };

                    }
                    connection.Close();
                }
            }
            return res;
        }


        public IList<Contrato> ObtenerVigentes(DateTime fechaahora)
        {
            IList<Contrato> res = new List<Contrato>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdContrato,c.IdInquilino,c.IdInmueble,Duracion,FechaInicio,FechaFinal,c.Precio," +
                     " i.Dni, t.Direccion, t.Precio" +
                    " FROM Contratos c INNER JOIN Inquilinos i ON i.IdInquilino = c.IdInquilino  " +
                    " INNER JOIN Inmuebles t ON c.IdInmueble = t.IdInmueble " +
                    " Where @fecha >= c.FechaInicio AND @fecha <= c.FechaFinal";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@fecha", SqlDbType.Date).Value = fechaahora;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {


                        Contrato c = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            IdInquilino = reader.GetInt32(1),
                            IdInmueble = reader.GetInt32(2),
                            Duracion = reader.GetInt32(3),
                            FechaInicio = reader.GetDateTime(4),
                            FechaFinal = reader.GetDateTime(5),
                            Precio = reader.GetDecimal(6),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(8),
                                Precio = reader.GetDecimal(9),
                            },
                            Inquilino = new Inquilino
                            {
                                dni = reader.GetString(7),
                            }


                        };
                        res.Add(c);
                    }
                    connection.Close();
                }
            }
            return res;
        }


        public IList<Contrato> ObtenerInmueblesLibres(DateTime fechaahora, DateTime fechafinal)
        {
            IList<Contrato> res = new List<Contrato>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdContrato,c.IdInquilino,c.IdInmueble,Duracion,FechaInicio,FechaFinal,c.Precio," +
                     " i.Dni, t.Direccion, t.Precio" +
                    " FROM Contratos c INNER JOIN Inquilinos i ON i.IdInquilino = c.IdInquilino  " +
                    " INNER JOIN Inmuebles t ON c.IdInmueble = t.IdInmueble " +
                    " Where t.IdInmueble NOT IN (SELECT t.IdInmueble FROM Contratos c INNER JOIN Inquilinos i ON i.IdInquilino = c.IdInquilino INNER JOIN Inmuebles t ON c.IdInmueble = t.IdInmueble  Where (@fecha >= c.FechaInicio AND @fecha <=c.FechaFinal) OR (@fecha2 >= c.FechaInicio AND @fecha2 <=c.FechaFinal))";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@fecha", SqlDbType.Date).Value = fechaahora;
                    command.Parameters.Add("@fecha2", SqlDbType.Date).Value = fechafinal;

                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {


                        Contrato c = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            IdInquilino = reader.GetInt32(1),
                            IdInmueble = reader.GetInt32(2),
                            Duracion = reader.GetInt32(3),
                            FechaInicio = reader.GetDateTime(4),
                            FechaFinal = reader.GetDateTime(5),
                            Precio = reader.GetDecimal(6),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(8),
                                Precio = reader.GetDecimal(9),
                            },
                            Inquilino = new Inquilino
                            {
                                dni = reader.GetString(7),
                            }


                        };
                        res.Add(c);
                    }
                    connection.Close();
                }
            }
            return res;
        }



        public IList<Contrato> ObtenerTodosLosContratosDeInmueble(String direccion)
        {
            IList<Contrato> res = new List<Contrato>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdContrato,c.IdInquilino,c.IdInmueble,Duracion,FechaInicio,FechaFinal,c.Precio," +
                     " i.Dni, t.Direccion, t.Precio" +
                    " FROM Contratos c INNER JOIN Inquilinos i ON i.IdInquilino = c.IdInquilino  " +
                    " INNER JOIN Inmuebles t ON c.IdInmueble = t.IdInmueble" +
                    " WHERE t.Direccion = @direccion";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@direccion", SqlDbType.VarChar).Value = direccion;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {


                        Contrato c = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            IdInquilino = reader.GetInt32(1),
                            IdInmueble = reader.GetInt32(2),
                            Duracion = reader.GetInt32(3),
                            FechaInicio = reader.GetDateTime(4),
                            FechaFinal = reader.GetDateTime(5),
                            Precio = reader.GetDecimal(6),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(8),
                                Precio = reader.GetDecimal(9),
                            },
                            Inquilino = new Inquilino
                            {
                                dni = reader.GetString(7),
                            }


                        };
                        res.Add(c);
                    }
                    connection.Close();
                }
            }
            return res;
        }


    }
}
