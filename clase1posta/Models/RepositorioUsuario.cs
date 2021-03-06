﻿using Microsoft.Extensions.Configuration;
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
        public int Baja(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM Usuario WHERE IdUsuario = @id";
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

        public IList<Usuario> ObtenerTodos()
        {
            IList<Usuario> res = new List<Usuario>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdUsuario, Nombre,Apellido,Email,Rol" +
                    $" FROM Usuario";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Usuario p = new Usuario
                        {
                            IdUsuario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Rol = reader.GetString(4),

                        };
                        res.Add(p);
                    }
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

        public Usuario ObtenerPorId(int id)
        {
            Usuario p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdUsuario, Nombre,Apellido,Email,Rol FROM Usuario" +
                    $" WHERE IdUsuario=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    try
                    {
                        if (reader.Read())
                        {
                            p = new Usuario
                            {
                                IdUsuario = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                Email = reader.GetString(3),
                                Rol = reader.GetString(4),

                            };
                        }
                        connection.Close();
                    }
                    catch(Exception ex)
                    {

                    }
                   
                }
            }
            return p;
        }


        public int Modificacion(Usuario p)
        {
            int j = 0;
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Usuario SET Nombre=@nombre,Apellido=@apellido,Email = @email,  Rol=@rol " +
                    $"WHERE IdUsuario = @idUsuario";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@email", p.Email);
                    command.Parameters.AddWithValue("@rol", p.Rol);
                    command.Parameters.AddWithValue("@idUsuario", p.IdUsuario);
                    connection.Open();
                    res = command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            return res;
        }
    }
}
