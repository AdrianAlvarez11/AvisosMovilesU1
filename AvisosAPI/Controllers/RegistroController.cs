using AvisosAPI.Models.DTOs;
using AvisosAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AvisosAPI.Models.DTOs.RegistroDTOs;

namespace AvisosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroController : ControllerBase
    {
        private readonly RegistroService service;

        public RegistroController(RegistroService service)
        {
            this.service = service;
        }

        [HttpPost("maestro")]
        [AllowAnonymous]
        public IActionResult RegistrarMaestro(MaestroRegistroDTO dto)
        {
            try
            {
                service.RegistrarMaestro(dto);
                return Ok("Maestro registrado correctamente.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);   // 409 si el NumControl ya existe
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("alumno")]
        [Authorize(Roles = "Maestro")]
        public IActionResult RegistrarAlumno(AlumnoRegistroDTO dto)
        {
            try
            {
                service.RegistrarAlumno(dto);
                return Ok("Alumno registrado correctamente.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);   
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
