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

        // pantalla principal del alumno.
    
        public List<AvisoPersonalResumenDTO> GetAvisosAlumno()
        {
            var idAlumno = ObtenerIdDesdeToken();

            var avisos = avisoRepository.Query()
                .Include(x => x.IdMaestroNavigation)
                .Include(x => x.IdEstadoNavigation)
                .Where(x => x.IdAlumno == idAlumno && x.Eliminado == false)
                .OrderByDescending(x => x.FechaEnviado)
                .ToList();

            var avisosMapeados = avisos.Select(x => mapper.Map<AvisoPersonalResumenDTO>(x)).ToList();

            //enviar los mapeados como estaban, pero cambiar de una vez el estado de nuevo a recibido para la proxima vez que los vea el alumno. 
            foreach (var a in avisos)
            {
                if (a.IdEstado == 1)
                {
                    a.IdEstado = 2;
                    avisoRepository.Update(a);
                }
                    
            }
            return avisosMapeados;
        }

        // Al abrirlo se registra FechaLeido y se cambia el estado a Leído (3)

        public AvisoPersonalDetalleDTO GetDetalle(int idAviso)
        {
            var idAlumno = ObtenerIdDesdeToken();

            var aviso = avisoRepository.Query()
                .Include(x => x.IdMaestroNavigation)
                .Include(x => x.IdEstadoNavigation)
                .FirstOrDefault(x => x.Id == idAviso && x.IdAlumno == idAlumno && x.Eliminado == false);

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

        //  Maestro: enviar aviso a un alumno 
        public void Crear(AvisoPersonalCreateDTO dto)
        {
            var idMaestro = ObtenerIdDesdeToken();

            var aviso = mapper.Map<Avisopersonal>(dto);
            aviso.IdMaestro = idMaestro;
            aviso.FechaEnviado = DateTime.Now;
            aviso.IdEstado = 1; // Nuevo
            aviso.Eliminado = false;

            avisoRepository.Insert(aviso);
        }

        //  Maestro: ver avisos enviados a un alumno específico 
        public List<AvisoPersonalResumenDTO> GetAvisosDeAlumno(int idAlumno)
        {
            var avisos = avisoRepository.Query()
                .Include(x => x.IdMaestroNavigation)
                .Include(x => x.IdEstadoNavigation)
                .Where(x => x.IdAlumno == idAlumno && x.Eliminado == false)
                .OrderByDescending(x => x.FechaEnviado)
                .ToList();

            return avisos.Select(x => mapper.Map<AvisoPersonalResumenDTO>(x)).ToList();
        }

        public void Eliminar(int idAviso)
        {
            var idMaestro = ObtenerIdDesdeToken();

            var aviso = avisoRepository.Query()
                .FirstOrDefault(x => x.Id == idAviso
                                  && x.IdMaestro == idMaestro
                                  && x.Eliminado == false);

            if (aviso == null)
                throw new KeyNotFoundException("Aviso no encontrado.");

            aviso.Eliminado = true;
            avisoRepository.Update(aviso);
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
