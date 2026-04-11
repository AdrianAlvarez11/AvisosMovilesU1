
using AutoMapper;
using AvisosAPI.Helpers;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static AvisosAPI.Models.DTOs.AuthDTOs;

namespace AvisosAPI.Services
{
    public class AuthService
    {
        private readonly Repository<Alumno> alumnoRepository;
        private readonly Repository<Maestro> maestroRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AuthService(
            Repository<Alumno> alumnoRepository,
            Repository<Maestro> maestroRepository,
            IMapper mapper,
            IConfiguration configuration)
        {
            this.alumnoRepository = alumnoRepository;
            this.maestroRepository = maestroRepository;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        public AlumnoLoginResponseDTO LoginAlumno(AlumnoLoginDTO dto)
        {
            var alumno = alumnoRepository.Query()
                .Include(x => x.IdGrupoNavigation)
                .FirstOrDefault(x => x.NumControl == dto.NumControl);

            if (alumno == null || !EncriptacionHelper.VerifySHA512HashWithSalt(dto.Contrasena, alumno.Contrasena))
            {
                throw new UnauthorizedAccessException("Credenciales incorrectas.");
            }

            var token = GenerarToken(alumno.Id, "Alumno");
            var response = mapper.Map<AlumnoLoginResponseDTO>(alumno);
            response.Token = token;
            return response;
        }

        public MaestroLoginResponseDTO LoginMaestro(MaestroLoginDTO dto)
        {
            var maestro = maestroRepository.Query()
                .Include(x => x.Grupo)
                .FirstOrDefault(x => x.NumControl == dto.NumControl);

            if (maestro == null || !EncriptacionHelper.VerifySHA512HashWithSalt(dto.Contrasena, maestro.Contrasena))
            {
                throw new UnauthorizedAccessException("Credenciales incorrectas.");
            }

            var token = GenerarToken(maestro.Id, "Maestro");
            var response = mapper.Map<MaestroLoginResponseDTO>(maestro);
            response.Token = token;
            return response;
        }

        private string GenerarToken(int id, string rol)
        {
            var claims = new[]
            {
                new Claim("Id", id.ToString()),
                new Claim(ClaimTypes.Role, rol)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

            var credenciales = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddHours(
                double.Parse(configuration["Jwt:ExpirationHours"]!));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiracion,
                signingCredentials: credenciales);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}