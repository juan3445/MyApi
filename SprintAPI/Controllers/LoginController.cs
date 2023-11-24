using Microsoft.AspNetCore.Mvc;
using SprintAPI.Data;
using SprintAPI.Models;


namespace SprintAPI.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UsuarioModel usuario)
        {
            var func = new LoginData();
            var userValido = await func.ValidarUsuario(usuario);
            if (userValido)
            {
                var token = func.GenerarToken(usuario.Cuenta);
                return Ok(new { Token = token });
            }
            else {
                return Unauthorized(); 
            }
        }
    }


}
