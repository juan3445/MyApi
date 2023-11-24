using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SprintAPI.Data;
using SprintAPI.Models;
using System.Text.RegularExpressions;
namespace SprintAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EstudianteController : ControllerBase
    {
        EstudianteData _log = new EstudianteData();

        [HttpGet]
        public async Task<ActionResult<List<EstudianteModel>>> GetAllEstudiantes()
        {
            var func = new EstudianteData();
            var lista = await func.GetEstudiante();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<EstudianteModel>>> GetEstudiante(string id)
        {
            if (!Regex.IsMatch(id, "^[0-9]+$"))
            {
                _log.Log($"HttpGet-BadRequest: El campo identificación debe contener únicamente números.");
                return BadRequest("El campo identificación debe contener únicamente números.");
            }

            var func = new EstudianteData();
            var estudiante = (await func.GetEstudianteById(id)).FirstOrDefault();

            if (estudiante == null)
            {
                _log.Log($"NotFound: El estudiante con documento {id} no existe.");
                return NotFound($"El estudiante con documento {id} no existe.");
            }

            return Ok(estudiante);
        }

        [HttpPost("v2/GET/")]
        public async Task<ActionResult<List<EstudianteModel>>> GetEstudiantes([FromBody]List<string> ids)
        {
            var func = new EstudianteData();
            var estudiante = await func.GetEstudiantes(ids);
            return Ok(estudiante);
        }

        [HttpPost]
        public async Task<ActionResult> PostEstudiante([FromBody]EstudianteModel estudiante)
        {     


            if (!Regex.IsMatch(estudiante.Identificacion, "^[0-9]+$"))
            {
                _log.Log($"BadRequest: El campo identificación debe contener únicamente números.");
                return BadRequest("El campo identificación debe contener únicamente números.");
            }

            var func = new EstudianteData();
            var estud = (await func.GetEstudianteById(estudiante.Identificacion)).FirstOrDefault();
            var esMunicipioValido = await func.ValidarCodigo(estudiante.Municipio, 1);
            var esEstadoCivilValido = await func.ValidarCodigo(estudiante.EstadoCivil, 2);
            var esGeneroValido = await func.ValidarCodigo(estudiante.Genero, 2);
            var esOcupacionValido = await func.ValidarCodigo(estudiante.Ocupacion, 2);
            var esViveConValido = await func.ValidarCodigo(estudiante.ViveCon, 2);
            var esNivelFormacionValido = await func.ValidarCodigo(estudiante.NivelFormacion, 2);
            var esEstratoValido = await func.ValidarCodigo(estudiante.Estrato, 2);

            if (!esMunicipioValido || !esEstadoCivilValido || !esGeneroValido || !esOcupacionValido || !esViveConValido || !esNivelFormacionValido || !esEstratoValido)
            {
                func.Log($"El código de municipio, EstadoCivil, Genero, Ocupacion, ViveCon, NivelFormacion o Estrato ingresado no es válido.");
                return BadRequest();
            }

            if (estud != null)
            {
                _log.Log($"NotFound: El estudiante con documento {estudiante.Identificacion} ya existe.");
                return NotFound($"El estudiante con documento {estudiante.Identificacion} ya existe.");
            }

            await func.PostEstudiante(estudiante);
            return CreatedAtAction("GetEstudiante",new { id = estudiante.Identificacion }, estudiante);
        }

        [HttpPost("v2/POST/")]
        public async Task<ActionResult> PostEstudiantes([FromBody] List<EstudianteModel> estudiantes)
        {
            var func = new EstudianteData();
            await func.PostEstudiantes(estudiantes);
            return Ok();
        }
    }
}
