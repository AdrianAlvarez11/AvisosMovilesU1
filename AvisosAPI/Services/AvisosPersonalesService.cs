using AutoMapper;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AvisosAPI.Services
{
    public class AvisosPersonalesService
    {
        private readonly Repository<Avisopersonal> avisoRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AvisosPersonalesService(
            Repository<Avisopersonal> avisoRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            this.avisoRepository = avisoRepository;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        // ── Alumno: obtener lista de sus avisos ──────────────────────
        // Se usa en la pantalla principal del alumno.
        // Incluimos navegaciones para que el mapper pueda leer
        // NombreMaestro y NombreEstado.
        public List<AvisoPersonalResumenDTO> GetAvisosAlumno()
        {
            var idAlumno = ObtenerIdDesdeToken();

            var avisos = avisoRepository.Query()
                .Include(x => x.IdMaestroNavigation)
                .Include(x => x.IdEstadoNavigation)
                .Where(x => x.IdAlumno == idAlumno)
                .OrderByDescending(x => x.FechaEnviado)
                .ToList();

            return avisos.Select(x => mapper.Map<AvisoPersonalResumenDTO>(x)).ToList();
        }

        // ── Alumno: abrir un aviso específico ────────────────────────
        // Al abrirlo se registra FechaLeido y se cambia el estado a Leído (3)

        public AvisoPersonalDetalleDTO GetDetalle(int idAviso)
        {
            var idAlumno = ObtenerIdDesdeToken();

            var aviso = avisoRepository.Query()
                .Include(x => x.IdMaestroNavigation)
                .Include(x => x.IdEstadoNavigation)
                .FirstOrDefault(x => x.Id == idAviso && x.IdAlumno == idAlumno);

            if (aviso == null)
                throw new KeyNotFoundException("Aviso no encontrado.");

            if (aviso.FechaLeido == null)
            {
                aviso.FechaLeido = DateTime.Now;
                aviso.IdEstado = 3; // Leído
                avisoRepository.Update(aviso);
            }

            return mapper.Map<AvisoPersonalDetalleDTO>(aviso);
        }

        // ── Maestro: enviar aviso a un alumno ───────────────────────
        public void Crear(AvisoPersonalCreateDTO dto)
        {
            var idMaestro = ObtenerIdDesdeToken();

            var aviso = mapper.Map<Avisopersonal>(dto);
            aviso.IdMaestro = idMaestro;
            aviso.FechaEnviado = DateTime.Now;
            aviso.IdEstado = 1; // Nuevo

            avisoRepository.Insert(aviso);
        }

        // ── Maestro: ver avisos enviados a un alumno específico ─────
        public List<AvisoPersonalResumenDTO> GetAvisosDeAlumno(int idAlumno)
        {
            var avisos = avisoRepository.Query()
                .Include(x => x.IdMaestroNavigation)
                .Include(x => x.IdEstadoNavigation)
                .Where(x => x.IdAlumno == idAlumno)
                .OrderByDescending(x => x.FechaEnviado)
                .ToList();

            return avisos.Select(x => mapper.Map<AvisoPersonalResumenDTO>(x)).ToList();
        }

        private int ObtenerIdDesdeToken()
        {
            var idStr = httpContextAccessor.HttpContext?
                .User.FindFirst("Id")?.Value;

            if (idStr == null)
                throw new UnauthorizedAccessException("No se pudo identificar al usuario.");

            return int.Parse(idStr);
        }
    }
}
