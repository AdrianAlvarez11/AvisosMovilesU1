using AvisosAPI.Models.DTOs;
using AvisosAPI.Services;
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

        public AvisosGeneralesController(AvisosGeneralesService service)
        {
            this.service = service;
        }

        // GET api/avisosgenerales
        // Alumno: lista de avisos vigentes
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

        // GET api/avisosgenerales/5
        // Alumno: detalle de un aviso vigente
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

        // POST api/avisosgenerales
        // Maestro: publica un aviso general
        [HttpPost]
        [Authorize(Roles = "Maestro")]
        public IActionResult Post(AvisoGeneralCreateDTO dto)
        {
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
    }
}

