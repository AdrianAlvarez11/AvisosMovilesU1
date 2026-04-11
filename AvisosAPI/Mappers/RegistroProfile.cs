using AutoMapper;
using AvisosAPI.Models.Entities;
using static AvisosAPI.Models.DTOs.RegistroDTOs;

namespace AvisosAPI.Mappers
{
    public class RegistroProfile : Profile
    {
        public RegistroProfile()
        {

            CreateMap<MaestroRegistroDTO, Maestro>()
                .ForMember(dest => dest.Contrasena, opt => opt.Ignore());

            CreateMap<AlumnoRegistroDTO, Alumno>()
                .ForMember(dest => dest.Contrasena, opt => opt.Ignore())
                .ForMember(dest => dest.IdGrupo, opt => opt.Ignore());
        }
    }
}