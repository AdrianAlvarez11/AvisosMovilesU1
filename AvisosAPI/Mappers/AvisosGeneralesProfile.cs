using AutoMapper;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;

namespace AvisosAPI.Mappers
{
    public class AvisosGeneralesProfile : Profile
    {
        public AvisosGeneralesProfile()
        {
            CreateMap<Avisogeneral, AvisoGeneralResumenDTO>()
                .ForMember(dest => dest.NombreMaestro,
                    opt => opt.MapFrom(src => src.IdMaestroNavigation.Nombre));

            // Detalle alumno — igual que antes, solo cambia el nombre del DTO
            CreateMap<Avisogeneral, AvisoGeneralDetalleAlumnoDTO>()
                .ForMember(dest => dest.NombreMaestro,
                    opt => opt.MapFrom(src => src.IdMaestroNavigation.Nombre));

            // Detalle maestro — las listas las construye el servicio manualmente
            // porque requieren filtrar por IdEstado, algo que AutoMapper
            // no puede hacer solo. Por eso las ignoramos aquí.
            CreateMap<Avisogeneral, AvisoGeneralDetalleMaestroDTO>()
                .ForMember(dest => dest.NombreMaestro,
                    opt => opt.MapFrom(src => src.IdMaestroNavigation.Nombre))
                .ForMember(dest => dest.IdMaestro,
                    opt => opt.MapFrom(src => src.IdMaestro))
                .ForMember(dest => dest.PendientesLectura, opt => opt.Ignore())
                .ForMember(dest => dest.Leidos, opt => opt.Ignore());
                

            CreateMap<Alumnoavisogeneral, AlumnoLecturaDTO>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.IdAlumnoNavigation.Id))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.IdAlumnoNavigation.Nombre))
                .ForMember(dest => dest.NumControl,
                    opt => opt.MapFrom(src => src.IdAlumnoNavigation.NumControl));

            CreateMap<AvisoGeneralCreateDTO, Avisogeneral>();
        }
    }
}