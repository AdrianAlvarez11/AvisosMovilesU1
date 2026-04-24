using AutoMapper;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AvisosAPI.Services
{
    public class GruposService
    {
        private readonly Repository<Grupo> grupoRepository;
        private readonly Repository<Alumno> alumnoRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GruposService(
            Repository<Grupo> grupoRepository,
            Repository<Alumno> alumnoRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            this.grupoRepository = grupoRepository;
            this.alumnoRepository = alumnoRepository;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        //pantalla principal del maestro.
        public GrupoDetalleDTO GetGrupoDelMaestro()
        {
            var idMaestro = ObtenerIdDesdeToken();

            var grupo = grupoRepository.Query()
                .Include(x => x.Alumno)
                .FirstOrDefault(x => x.IdMaestro == idMaestro);

            if (grupo == null)
                throw new KeyNotFoundException("No se encontró un grupo asociado al maestro.");

            return mapper.Map<GrupoDetalleDTO>(grupo);
        }

        //detalle de un alumno específico
        public AlumnoDetalleDTO GetAlumno(int idAlumno)
        {
            var idMaestro = ObtenerIdDesdeToken();

            // Primero obtenemos el grupo del maestro para validar
            var grupo = grupoRepository.Query()
                .FirstOrDefault(x => x.IdMaestro == idMaestro);

            if (grupo == null)
                throw new KeyNotFoundException("No se encontró un grupo asociado al maestro.");

            var alumno = alumnoRepository.Query()
                .Include(x => x.IdGrupoNavigation)
                .Include(x => x.Avisopersonal.Where(x=>x.Eliminado == false))
                    .ThenInclude(x => x.IdMaestroNavigation)
                .Include(x => x.Avisopersonal.Where(x=>x.Eliminado == false))
                    .ThenInclude(x => x.IdEstadoNavigation)
                .FirstOrDefault(x => x.Id == idAlumno && x.IdGrupo == grupo.Id && x.Eliminado == false);

            if (alumno == null)
                throw new KeyNotFoundException("Alumno no encontrado.");

            return mapper.Map<AlumnoDetalleDTO>(alumno);
        }

        public void EliminarAlumno(int idAlumno)
        {
            var idMaestro = ObtenerIdDesdeToken();

            var alumno = alumnoRepository.Query()
                .FirstOrDefault(x => x.Id == idAlumno
                                  && x.Eliminado == false);

            if (alumno == null)
                throw new KeyNotFoundException("Alumno no encontrado.");

            alumno.Eliminado = true;
            alumnoRepository.Update(alumno);
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