
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

        public LoginResponseDTO Login(LoginDTO dto)
        {
            if (dto.NumControl.Length == 8)
            {
                var alumno = alumnoRepository.Query()
                    .Include(x => x.IdGrupoNavigation)
                    .FirstOrDefault(x => x.NumControl == dto.NumControl);

                if (alumno == null || !EncriptacionHelper.VerifySHA512HashWithSalt(dto.Contrasena, alumno.Contrasena))
                {
                    throw new UnauthorizedAccessException("Credenciales incorrectas.");
                }

                var token = GenerarToken(alumno.Id, "Alumno", alumno.Nombre);
                var response = mapper.Map<LoginResponseDTO>(alumno);
                response.Token = token;
                response.Rol = "Alumno";
                return response;
            }
            else if (dto.NumControl.Length == 4)
            {
                var maestro = maestroRepository.Query()
                    .Include(x => x.Grupo)
                    .FirstOrDefault(x => x.NumControl == dto.NumControl);

                if (maestro == null || !EncriptacionHelper.VerifySHA512HashWithSalt(dto.Contrasena, maestro.Contrasena))
                {
                    throw new UnauthorizedAccessException("Credenciales incorrectas.");
                }

                var token = GenerarToken(maestro.Id, "Maestro", maestro.Nombre);
                var response = mapper.Map<LoginResponseDTO>(maestro);
                response.Token = token;
                response.Rol = "Maestro";
                return response;
            }
            else
            {
                throw new UnauthorizedAccessException("El número de control es inválido.");
            }
        }

        private string GenerarToken(int id, string rol, string Nombre)
        {
            var claims = new[]
            {
                new Claim("Id", id.ToString()),
                new Claim(ClaimTypes.Role, rol),
                new Claim(ClaimTypes.Name, Nombre),
            };

            var key = configuration.GetValue<string>("Jwt:SecretKey");


            

            var token = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("Jwt:Issuer"),
                audience: configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? "")), SecurityAlgorithms.HmacSha256)
);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}