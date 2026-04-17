using AutoMapper;
using AvisosAPI.Helpers;
using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using System.Text.RegularExpressions;
using static AvisosAPI.Models.DTOs.RegistroDTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AvisosAPI.Services
{
    public class RegistroService
    {
        private readonly Repository<Maestro> maestroRepository;
        private readonly Repository<Alumno> alumnoRepository;
        private readonly Repository<Grupo> grupoRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RegistroService(
            Repository<Maestro> maestroRepository,
            Repository<Alumno> alumnoRepository,
            Repository<Grupo> grupoRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            this.maestroRepository = maestroRepository;
            this.alumnoRepository = alumnoRepository;
            this.grupoRepository = grupoRepository;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public void RegistrarMaestro(MaestroRegistroDTO dto)
        {
            // Verificar que el NumControl no esté ya registrado
            var existe = maestroRepository.Query()
                .Any(x => x.NumControl == dto.NumControl);

            if (existe)
                throw new InvalidOperationException("El número de control ya está registrado.");

            if (!Regex.IsMatch(dto.NumControl, @"^[0-9]{4}$"))
                throw new InvalidOperationException("El número de control no tiene un formato válido.");

            var maestro = mapper.Map<Maestro>(dto);
            maestro.Contrasena = EncriptacionHelper.ComputeSHA512HashWithSalt(dto.Contrasena);

            maestroRepository.Insert(maestro);

            var grupo = new Grupo
            {
                Nombre = dto.NombreGrupo,
                IdMaestro = maestro.Id  
            };

            grupoRepository.Insert(grupo);
        }

        // Alta de alumno requiere token de maestro.
        // El IdGrupo se extrae del token para que el maestro no pueda
        // asignar alumnos a un grupo que no sea el suyo.
        public void RegistrarAlumno(AlumnoRegistroDTO dto)
        {
            // Leer el Id del maestro desde los claims del token JWT
            var idMaestroStr = httpContextAccessor.HttpContext?
                .User.FindFirst("Id")?.Value;

            if (idMaestroStr == null)
                throw new UnauthorizedAccessException("No se pudo identificar al maestro.");

            var idMaestro = int.Parse(idMaestroStr);

            var grupo = grupoRepository.Query()
                .FirstOrDefault(x => x.IdMaestro == idMaestro);

            if (grupo == null)
                throw new KeyNotFoundException("No se encontró un grupo asociado al maestro.");

            if (!Regex.IsMatch(dto.NumControl.ToUpper(), @"^[0-9]{2}1[AGDTPMQV][ED0-9][0-9]{3}$"))
                throw new InvalidOperationException("El número de control no tiene un formato válido.");

            var existe = alumnoRepository.Query()
                .Any(x => x.NumControl == dto.NumControl);

            if (existe)
                throw new InvalidOperationException("El número de control ya está registrado.");

            var alumno = mapper.Map<Alumno>(dto);
            alumno.Contrasena = EncriptacionHelper.ComputeSHA512HashWithSalt(dto.Contrasena);
            alumno.IdGrupo = grupo.Id;
            alumno.Eliminado = false;

            alumnoRepository.Insert(alumno);
        }
    }
}
