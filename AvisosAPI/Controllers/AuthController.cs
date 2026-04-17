using AvisosAPI.Models.DTOs;
using AvisosAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using static AvisosAPI.Models.DTOs.AuthDTOs;

namespace AvisosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService service;
        private readonly IValidator<AlumnoLoginDTO> alumnoValidator;
        private readonly IValidator<MaestroLoginDTO> maestroValidator;

        public AuthController(AuthService service, IValidator<AlumnoLoginDTO> alumnoValidator, IValidator<MaestroLoginDTO> maestroValidator)
        {
            this.service = service;
            this.alumnoValidator = alumnoValidator;
            this.maestroValidator = maestroValidator;
        }

        [HttpPost("alumno")]
        public IActionResult LoginAlumno(AlumnoLoginDTO dto)
        {
            var result = alumnoValidator.Validate(dto);
            if (!result.IsValid) return BadRequest(result.Errors.Select(x => x.ErrorMessage));

            try
            {
                var response = service.LoginAlumno(dto);
                return Ok(response);
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

        [HttpPost("maestro")]
        public IActionResult LoginMaestro(MaestroLoginDTO dto)
        {
            var result = maestroValidator.Validate(dto);
            if (!result.IsValid) return BadRequest(result.Errors.Select(x => x.ErrorMessage));

            try
            {
                var response = service.LoginMaestro(dto);
                return Ok(response);
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
    }
}