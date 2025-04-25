using BiblioAPI.Models;
using BiblioAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace BiblioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioModel>>> ObtenerUsuarios()
        {
            var usuarios = await _usuarioService.ObtenerUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioModel>> ObtenerUsuarioPorId(int id)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult> CrearUsuario([FromBody] UsuarioModel usuario)
        {
            await _usuarioService.CrearUsuarioAsync(usuario);
            return CreatedAtAction(nameof(ObtenerUsuarioPorId), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarUsuario(int id, [FromBody] UsuarioModel usuario)
        {
            var actualizado = await _usuarioService.ActualizarUsuarioAsync(id, usuario);
            if (!actualizado)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarUsuario(int id)
        {
            var eliminado = await _usuarioService.EliminarUsuarioAsync(id);
            if (!eliminado)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("validar")]
        public async Task<ActionResult<UsuarioModel>> ValidarUsuario([FromQuery] string correo, [FromQuery] string clave)
        {
            var usuario = await _usuarioService.ValidarUsuarioAsync(correo, clave);
            if (usuario == null)
            {
                return Unauthorized(); // 401 si no se encuentra el usuario
            }
            return Ok(usuario); // Retorna el usuario si la validación es exitosa
        }
          
    }
}
