using AutoMapper;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;

namespace AvisosAPI.Mappers
{
    public class GruposProfile : Profile
    {
        public GruposProfile()
        {
            // Alumno -> AlumnoResumenDTO
            // Id, NumControl y Nombre coinciden: mapeo automático.
            CreateMap<Alumno, AlumnoResumenDTO>();

            // Grupo -> GrupoDetalleDTO
            // "Alumnos" en el DTO viene de la colección "Alumno" en la entidad
            // (EF scaffold la nombró en singular). También se mapea cada Alumno
            // a AlumnoResumenDTO usando el mapeo que ya definimos arriba.
            CreateMap<Grupo, GrupoDetalleDTO>()
                .ForMember(dest => dest.Alumnos,
                    opt => opt.MapFrom(src => src.Alumno));

            // Alumno -> AlumnoDetalleDTO
            // NombreGrupo viene de la navegación.
            // Avisos viene de la colección Avisopersonal, mapeada a resumen.
            CreateMap<Alumno, AlumnoDetalleDTO>()
                .ForMember(dest => dest.NombreGrupo,
                    opt => opt.MapFrom(src => src.IdGrupoNavigation.Nombre))
                .ForMember(dest => dest.Avisos,
                    opt => opt.MapFrom(src => src.Avisopersonal));
        }
    }
}
