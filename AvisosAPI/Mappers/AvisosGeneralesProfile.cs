using AutoMapper;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;

namespace AvisosAPI.Mappers
{
    public class AvisosGeneralesProfile : Profile
    {
        public AvisosGeneralesProfile()
        {
            // Entidad -> DTO Resumen
            CreateMap<Avisogeneral, AvisoGeneralResumenDTO>()
                .ForMember(dest => dest.NombreMaestro,
                    opt => opt.MapFrom(src => src.IdMaestroNavigation.Nombre));

            // Entidad -> DTO Detalle
            CreateMap<Avisogeneral, AvisoGeneralDetalleDTO>()
                .ForMember(dest => dest.NombreMaestro,
                    opt => opt.MapFrom(src => src.IdMaestroNavigation.Nombre));

            // DTO Create -> Entidad
            // IdMaestro y FechaEnviado los asigna el servicio.
            CreateMap<AvisoGeneralCreateDTO, Avisogeneral>();
        }
    }
}
