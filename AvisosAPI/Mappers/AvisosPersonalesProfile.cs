using AutoMapper;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;

namespace AvisosAPI.Mappers
{
    public class AvisosPersonalesProfile : Profile
    {
        public AvisosPersonalesProfile()
        {
            // Entidad -> DTO Resumen (para la lista)
            // NombreMaestro y NombreEstado vienen de propiedades de navegación,
            // el resto de campos coincide en nombre y se mapea automático.
            CreateMap<Avisopersonal, AvisoPersonalResumenDTO>()
                .ForMember(dest => dest.NombreMaestro,
                    opt => opt.MapFrom(src => src.IdMaestroNavigation.Nombre))
                .ForMember(dest => dest.NombreEstado,
                    opt => opt.MapFrom(src => src.IdEstadoNavigation.Nombre));

            // Entidad -> DTO Detalle (para cuando se abre un aviso)
            
            CreateMap<Avisopersonal, AvisoPersonalDetalleDTO>()
                .ForMember(dest => dest.NombreMaestro,
                    opt => opt.MapFrom(src => src.IdMaestroNavigation.Nombre))
                .ForMember(dest => dest.NombreEstado,
                    opt => opt.MapFrom(src => src.IdEstadoNavigation.Nombre));

            // DTO Create -> Entidad (cuando el maestro crea un aviso)
            // IdMaestro, FechaEnviado e IdEstado NO vienen del DTO —
            // los asigna el servicio manualmente antes de guardar.
            CreateMap<AvisoPersonalCreateDTO, Avisopersonal>();
        }
    }
}
