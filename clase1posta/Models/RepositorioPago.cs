using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace clase1posta.Models
{
    public class RepositorioPago
    {
        private readonly string connectionString;
        private readonly IConfiguration configuration;

        public RepositorioPago(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:DefaultConnection"];

        }

        public int Alta(Pago p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Pagos (IdContrato,Cuota,Estado) " +
                    $"VALUES (@idContrato,@cuota,@estado);" +
                    $"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@idContrato", p.IdContrato);
                    command.Parameters.AddWithValue("@cuota", p.Cuota);
                    command.Parameters.AddWithValue("@estado", p.Estado);
               

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.IdPago = res;
                    connection.Close();
                }
            }
            return res;
        }


        public IList<Pago> ObtenerTodosPagosDe(int id)
        {
            IList<Pago> res = new List<Pago>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPago,IdContrato,Cuota,Estado,FechaPago FROM Pagos" +
                    $" WHERE IdContrato = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                       Pago p = new Pago
                        {
                            IdPago = reader.GetInt32(0),
                            IdContrato = reader.GetInt32(1),
                            Cuota = reader.GetInt32(2),
                            Estado = reader.GetBoolean(3),
                            
                            

                        };
                        if(!reader.IsDBNull(4))
                        {
                            p.FechaPago = reader.GetDateTime(4);
                        }
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }


        public int Pagar(Pago entidad)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Pagos SET FechaPago = @fechapago, Estado = @estado " +
                    "WHERE IdPago = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fechapago", entidad.FechaPago);
                    command.Parameters.AddWithValue("@id", entidad.IdPago);
                    command.Parameters.AddWithValue("@estado", entidad.Estado);

                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Pago ObtenerPorId(int id)
        {
            Pago res = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPago,IdContrato,Cuota,Estado,FechaPago FROM Pagos" +
                    $" WHERE IdPago = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res = new Pago
                        {
                            IdPago = reader.GetInt32(0),
                            IdContrato = reader.GetInt32(1),
                            Cuota = reader.GetInt32(2),
                            Estado = reader.GetBoolean(3),



                        };
                        if (!reader.IsDBNull(4))
                        {
                            res.FechaPago = reader.GetDateTime(4);
                        }
                        
                    }
                    connection.Close();
                }
            }
            return res;
        }








    }
}
