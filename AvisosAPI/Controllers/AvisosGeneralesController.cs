using AvisosAPI.Models.DTOs;
using AvisosAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AvisosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AvisosGeneralesController : ControllerBase
    {
        private readonly AvisosGeneralesService service;
        private readonly IValidator<AvisoGeneralCreateDTO> validator;

        public AvisosGeneralesController(AvisosGeneralesService service, IValidator<AvisoGeneralCreateDTO> validator)
        {
            this.service = service;
            this.validator = validator;
        }

        [HttpGet]
        [Authorize(Roles = "Alumno")]
        public IActionResult Get()
        {
            try
            {
                var avisos = service.GetVigentes();
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


        [HttpGet("alumno/{idAviso}")]
        [Authorize(Roles = "Alumno")]
        public IActionResult GetDetalleAlumno(int idAviso)
        {
            try
            {
                var aviso = service.GetDetalleAlumno(idAviso);
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

        [HttpGet("maestro/{idAviso}")]
        [Authorize(Roles = "Maestro")]
        public IActionResult GetDetalleMaestro(int idAviso)
        {
            try
            {
                var aviso = service.GetDetalleMaestro(idAviso);
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
        public IActionResult Post(AvisoGeneralCreateDTO dto)
        {
            var result = validator.Validate(dto);
            if (!result.IsValid) return BadRequest(result.Errors.Select(x => x.ErrorMessage));

            try
            {
                service.Crear(dto);
                return Ok("Aviso general publicado correctamente.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
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
