using AvisosAPI.Models.DTOs;
using AvisosAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AvisosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Maestro")]
    public class GruposController : ControllerBase
    {
        private readonly GruposService service;

        public GruposController(GruposService service)
        {
            this.service = service;
        }

        //obtiene su grupo con la lista completa de alumnos
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var grupo = service.GetGrupoDelMaestro();
                return Ok(grupo);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //detalle de un alumno específico de su grupo
        [HttpGet("alumno/{idAlumno}")]
        public IActionResult GetAlumno(int idAlumno)
        {
            try
            {
                var alumno = service.GetAlumno(idAlumno);
                return Ok(alumno);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}