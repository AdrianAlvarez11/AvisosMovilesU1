using AvisosAPI.Models.DTOs;
using AvisosAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvisosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // todos los endpoints de este controller requieren token
    public class AvisosPersonalesController : ControllerBase
    {
        private readonly AvisosPersonalesService service;
        private readonly IValidator<AvisoPersonalCreateDTO> validator;

        public AvisosPersonalesController(AvisosPersonalesService service, IValidator<AvisoPersonalCreateDTO> validator)
        {
            this.service = service;
            this.validator = validator;
        }

        // GET api/avisospersonales
        // Alumno: obtiene su lista de avisos
        [HttpGet]
        [Authorize(Roles = "Alumno")]
        public IActionResult Get()
        {
            try
            {
                var avisos = service.GetAvisosAlumno();
                return Ok(avisos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/avisospersonales/5
        // Alumno: abre un aviso — marca como leído automáticamente
        [HttpGet("{idAviso}")]
        [Authorize(Roles = "Alumno")]
        public IActionResult GetDetalle(int idAviso)
        {
            try
            {
                var aviso = service.GetDetalle(idAviso);
                return Ok(aviso);
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

        [HttpPost]
        [Authorize(Roles = "Maestro")]
        public IActionResult Post(AvisoPersonalCreateDTO dto)
        {
            var result = validator.Validate(dto);
            if (!result.IsValid) return BadRequest(result.Errors.Select(x => x.ErrorMessage));

            try
            {
                service.Crear(dto);
                return Ok("Aviso enviado correctamente.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("alumno/{idAlumno}")]
        [Authorize(Roles = "Maestro")]
        public IActionResult GetDeAlumno(int idAlumno)
        {
            try
            {
                var avisos = service.GetAvisosDeAlumno(idAlumno);
                return Ok(avisos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{idAviso}")]
        [Authorize(Roles = "Maestro")]
        public IActionResult Delete(int idAviso)
        {
            try
            {
                service.Eliminar(idAviso);
                return Ok("Aviso eliminado correctamente.");
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
