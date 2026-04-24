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
        private readonly IValidator<LoginDTO> loginValidator;

        public AuthController(AuthService service, IValidator<LoginDTO> loginValidator)
        {
            this.service = service;
            this.loginValidator = loginValidator;
        }

        [HttpPost]
        public IActionResult Login(LoginDTO dto)
        {
            var result = loginValidator.Validate(dto);
            if (!result.IsValid) return BadRequest(result.Errors.Select(x => x.ErrorMessage));

            try
            {
                var response = service.Login(dto);
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