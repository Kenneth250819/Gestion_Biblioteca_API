using BiblioAPI.Models;
using System.Data.SqlClient;
using System.Data;

namespace BiblioAPI.Services
{
    public class UsuarioService
    {
        private readonly string _connectionString;

        public UsuarioService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MiConexion");
        }

        public async Task<List<UsuarioModel>> ObtenerUsuariosAsync()
        {
            var usuarios = new List<UsuarioModel>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerUsuarios", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await con.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarios.Add(new UsuarioModel
                            {
                                Id = (int)reader["Id"],
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                Correo = reader["Correo"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                TipoUsuario = reader["TipoUsuario"].ToString(),
                                Clave = reader["Clave"].ToString()
                            });
                        }
                    }
                }
            }

            return usuarios;
        }

        public async Task<UsuarioModel> ObtenerUsuarioPorIdAsync(int id)
        {
            UsuarioModel usuario = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerUsuarios", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await con.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if ((int)reader["Id"] == id)
                            {
                                usuario = new UsuarioModel
                                {
                                    Id = (int)reader["Id"],
                                    Nombre = reader["Nombre"].ToString(),
                                    Apellido = reader["Apellido"].ToString(),
                                    Correo = reader["Correo"].ToString(),
                                    Telefono = reader["Telefono"].ToString(),
                                    TipoUsuario = reader["TipoUsuario"].ToString(),
                                    Clave = reader["Clave"].ToString()
                                };
                                break;
                            }
                        }
                    }
                }
            }

            return usuario;
        }

        public async Task CrearUsuarioAsync(UsuarioModel usuario)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@TipoUsuario", usuario.TipoUsuario);
                    cmd.Parameters.AddWithValue("@Clave", usuario.Clave);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> ActualizarUsuarioAsync(int id, UsuarioModel usuario)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ActualizarUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@TipoUsuario", usuario.TipoUsuario);
                    cmd.Parameters.AddWithValue("@Clave", usuario.Clave);

                    await con.OpenAsync();
                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0; // true si se actualizó, false si no se encontró
                }
            }
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EliminarUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    await con.OpenAsync();
                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0; // true si se eliminó, false si no se encontró
                }
            }
        }

        public async Task<UsuarioModel> ValidarUsuarioAsync(string correo, string clave)
        {
            UsuarioModel usuario = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ValidarUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Correo", correo);
                    cmd.Parameters.AddWithValue("@Clave", clave);

                    await con.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            usuario = new UsuarioModel
                            {
                                Id = (int)reader["Id"],
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                Correo = reader["Correo"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                TipoUsuario = reader["TipoUsuario"].ToString(),
                                Clave = reader["Clave"].ToString()
                            };
                        }
                    }
                }
            } 
            return usuario; // Retorna null si no se encuentra el usuario
        } 

    }
}
