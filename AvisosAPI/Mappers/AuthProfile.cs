using AutoMapper;
using AvisosAPI.Models.Entities;
using static AvisosAPI.Models.DTOs.AuthDTOs;

namespace AvisosAPI.Mappers
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            // Alumno -> LoginResponseDTO
            // NombreGrupo viene de navegación. Token NO se mapea aquí —
            // lo genera el servicio de auth y lo asigna después del mapeo.
            CreateMap<Alumno, LoginResponseDTO>()
                .ForMember(dest => dest.NombreGrupo,
                    opt => opt.MapFrom(src => src.IdGrupoNavigation.Nombre))
                .ForMember(dest => dest.Token,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Rol,
                    opt => opt.Ignore()); // se asigna manualmente en AuthService

            // Maestro -> LoginResponseDTO
            // IdGrupo y NombreGrupo vienen de la colección Grupo —
            // un maestro tiene un solo grupo, tomamos el primero.
            CreateMap<Maestro, LoginResponseDTO>()
                .ForMember(dest => dest.IdGrupo,
                    opt => opt.MapFrom(src => src.Grupo.First().Id))
                .ForMember(dest => dest.NombreGrupo,
                    opt => opt.MapFrom(src => src.Grupo.First().Nombre))
                .ForMember(dest => dest.Token,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Rol,
                    opt => opt.Ignore());
        }
    }
}
