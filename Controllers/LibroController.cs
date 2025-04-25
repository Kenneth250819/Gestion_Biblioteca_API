using BiblioAPI.Models;
using BiblioAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;

namespace BiblioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibroController : Controller
    {
        private readonly LibroService _libroService;

        public LibroController(LibroService libroService)
        {
            _libroService = libroService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroModel>>> GetLibros()
        {
            var libros = await _libroService.ObtenerLibrosAsync();
            return Ok(libros);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LibroModel>> GetLibroPorId(int id)
        {
            var libro = await _libroService.ObtenerLibroPorIdAsync(id);
            if (libro == null)
                return NotFound("Libro no encontrado");
            return Ok(libro);
        }

        // GET: api/Libro/buscar?titulo=xxx&autor=yyy
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<LibroModel>>> BuscarLibros([FromQuery] string? titulo, [FromQuery] string? autor)
        {
            var libros = await _libroService.BuscarLibrosAsync(titulo, autor);
            return Ok(libros);
        }

        [HttpPost]
        public async Task<ActionResult> CrearLibro([FromBody] LibroModel libro)
        {
            await _libroService.CrearLibroAsync(libro);
            return Ok("Libro agregado correctamente");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarLibro(int id, [FromBody] LibroModel libro)
        {
            var resultado = await _libroService.ActualizarLibroAsync(id, libro);
            if (!resultado)
                return NotFound("Libro no encontrado");
            return Ok("Libro actualizado correctamente");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarLibro(int id)
        {
            var resultado = await _libroService.EliminarLibroAsync(id);
            if (!resultado)
                return NotFound("Libro no encontrado");
            return Ok("Libro eliminado");
        }


    }
}
