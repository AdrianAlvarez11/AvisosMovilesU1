using AvisosAPI.Models.DTOs;
using AvisosAPI.Services;
using Microsoft.AspNetCore.Mvc;
using static AvisosAPI.Models.DTOs.AuthDTOs;

namespace AvisosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService service;

        public AuthController(AuthService service)
        {
            this.service = service;
        }

        [HttpPost("alumno")]
        public IActionResult LoginAlumno(AlumnoLoginDTO dto)
        {
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