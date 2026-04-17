using AutoMapper;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AvisosAPI.Services
{
    public class AvisosGeneralesService
    {
        private readonly Repository<Avisogeneral> avisoRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AvisosGeneralesService(
            Repository<Avisogeneral> avisoRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            this.avisoRepository = avisoRepository;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }


        public List<AvisoGeneralResumenDTO> GetVigentes()
        {
            var avisos = avisoRepository.Query()
                .Include(x => x.IdMaestroNavigation)
                .Where(x => x.FechaExpira > DateTime.Now && x.Eliminado == false)
                .OrderByDescending(x => x.FechaEnviado)
                .ToList();

            return avisos.Select(x => mapper.Map<AvisoGeneralResumenDTO>(x)).ToList();
        }


        public AvisoGeneralDetalleDTO GetDetalle(int idAviso)
        {
            var aviso = avisoRepository.Query()
                .Include(x => x.IdMaestroNavigation)
                .FirstOrDefault(x => x.Id == idAviso
                                  && x.FechaExpira > DateTime.Now
                                  && x.Eliminado == false);

            if (aviso == null)
                throw new KeyNotFoundException("Aviso no encontrado o no vigente.");

            return mapper.Map<AvisoGeneralDetalleDTO>(aviso);
        }


        public void Crear(AvisoGeneralCreateDTO dto)
        {
            if (dto.FechaExpira <= DateTime.Now)
                throw new InvalidOperationException("La fecha de expiración debe ser futura.");

            var idMaestro = ObtenerIdDesdeToken();

            var aviso = mapper.Map<Avisogeneral>(dto);
            aviso.IdMaestro = idMaestro;
            aviso.FechaEnviado = DateTime.Now;
            aviso.Eliminado = false;

            avisoRepository.Insert(aviso);
        }


        // Verifica que el aviso pertenezca al maestro del token.
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