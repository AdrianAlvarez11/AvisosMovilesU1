using AvisosAPI.Models.DTOs;
using AvisosAPI.Services;
using FluentValidation;
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
        private readonly IValidator<MaestroRegistroDTO> maestroValidator;
        private readonly IValidator<AlumnoRegistroDTO> alumnoValidator;

        public RegistroController(RegistroService service, IValidator<MaestroRegistroDTO> maestroValidator, IValidator<AlumnoRegistroDTO> alumnoValidator)
        {
            this.service = service;
            this.maestroValidator = maestroValidator;
            this.alumnoValidator = alumnoValidator;
        }

        [HttpPost("maestro")]
        [AllowAnonymous]
        public IActionResult RegistrarMaestro(MaestroRegistroDTO dto)
        {
            var result = maestroValidator.Validate(dto);
            if (!result.IsValid) return BadRequest(result.Errors.Select(x => x.ErrorMessage));

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
            var result = alumnoValidator.Validate(dto);
            if (!result.IsValid) return BadRequest(result.Errors.Select(x => x.ErrorMessage));

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
