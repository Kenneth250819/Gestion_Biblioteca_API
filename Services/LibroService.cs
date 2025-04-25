using BiblioAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace BiblioAPI.Services
{
    public class LibroService
    {
        private readonly string _connectionString;

        public LibroService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MiConexion");
        }

        public async Task<List<LibroModel>> ObtenerLibrosAsync()
        {
            var libros = new List<LibroModel>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerLibros", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await con.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            libros.Add(new LibroModel
                            {
                                Id = (int)reader["Id"],
                                Titulo = reader["Titulo"].ToString(),
                                Autor = reader["Autor"].ToString(),
                                Editorial = reader["Editorial"].ToString(),
                                ISBN = reader["ISBN"].ToString(),
                                Anio = (int)reader["Anio"],
                                Categoria = reader["Categoria"].ToString(),
                                Existencias = (int)reader["Existencias"]
                            });
                        }
                    }
                }
            }

            return libros;
        }

        public async Task<LibroModel> ObtenerLibroPorIdAsync(int id)
        {
            LibroModel libro = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerLibros", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await con.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if ((int)reader["Id"] == id)
                            {
                                libro = new LibroModel
                                {
                                    Id = (int)reader["Id"],
                                    Titulo = reader["Titulo"].ToString(),
                                    Autor = reader["Autor"].ToString(),
                                    Editorial = reader["Editorial"].ToString(),
                                    ISBN = reader["ISBN"].ToString(),
                                    Anio = (int)reader["Anio"],
                                    Categoria = reader["Categoria"].ToString(),
                                    Existencias = (int)reader["Existencias"]
                                };
                                break;
                            }
                        }
                    }
                }
            }

            return libro;
        }
        public async Task<List<LibroModel>> BuscarLibrosAsync(string? titulo, string? autor)
        {
            var libros = new List<LibroModel>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("spBuscarLibros", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Titulo", (object?)titulo ?? DBNull.Value);
                command.Parameters.AddWithValue("@Autor", (object?)autor ?? DBNull.Value);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        libros.Add(new LibroModel
                        {
                            Id = (int)reader["Id"],
                            Titulo = reader["Titulo"].ToString(),
                            Autor = reader["Autor"].ToString(),
                            Editorial = reader["Editorial"].ToString(),
                            ISBN = reader["ISBN"].ToString(),
                            Anio = Convert.ToInt32(reader["Anio"]),
                            Categoria = reader["Categoria"].ToString(),
                            Existencias = Convert.ToInt32(reader["Existencias"])
                        });

                    }
                }
            }

            return libros;
        }


        public async Task CrearLibroAsync(LibroModel libro)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarLibro", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Titulo", libro.Titulo);
                    cmd.Parameters.AddWithValue("@Autor", libro.Autor);
                    cmd.Parameters.AddWithValue("@Editorial", libro.Editorial);
                    cmd.Parameters.AddWithValue("@ISBN", libro.ISBN);
                    cmd.Parameters.AddWithValue("@Anio", libro.Anio);
                    cmd.Parameters.AddWithValue("@Categoria", libro.Categoria);
                    cmd.Parameters.AddWithValue("@Existencias", libro.Existencias);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> ActualizarLibroAsync(int id, LibroModel libro)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ActualizarLibro", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Titulo", libro.Titulo);
                    cmd.Parameters.AddWithValue("@Autor", libro.Autor);
                    cmd.Parameters.AddWithValue("@Editorial", libro.Editorial);
                    cmd.Parameters.AddWithValue("@ISBN", libro.ISBN);
                    cmd.Parameters.AddWithValue("@Anio", libro.Anio);
                    cmd.Parameters.AddWithValue("@Categoria", libro.Categoria);
                    cmd.Parameters.AddWithValue("@Existencias", libro.Existencias);

                    await con.OpenAsync();
                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0; // true si se actualizó, false si no se encontró
                }
            }
        }

        public async Task<bool> EliminarLibroAsync(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EliminarLibro", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    await con.OpenAsync();
                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0; // true si se eliminó, false si no se encontró
                }
            }
        }
    }
}
