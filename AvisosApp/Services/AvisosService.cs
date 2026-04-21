using AvisosApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using static AvisosApp.Models.DTOs.AuthDTOs;
using static AvisosApp.Models.DTOs.RegistroDTOs;

namespace AvisosApp.Services
{
    public class AvisosService
    {
        string baseUrl = "https://localhost:9123/";
        HttpClient client;

        public AvisosService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        // AUTH
        public async Task<AlumnoLoginResponseDTO?> LoginAlumno(AlumnoLoginDTO dto)
        {
            var response = await client.PostAsJsonAsync("api/auth/alumno", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AlumnoLoginResponseDTO>();
            }

            return null;
        }

        public async Task<MaestroLoginResponseDTO?> LoginMaestro(MaestroLoginDTO dto)
        {
            var response = await client.PostAsJsonAsync("api/auth/maestro", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MaestroLoginResponseDTO>();
            }

            return null;
        }

        // Registro
        public async Task<bool> RegistrarMaestro(MaestroRegistroDTO dto)
        {
            var response = await client.PostAsJsonAsync("api/registro/maestro", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RegistrarAlumno(AlumnoRegistroDTO dto)
        {
            var response = await client.PostAsJsonAsync("api/registro/alumno", dto);
            return response.IsSuccessStatusCode;
        }

        // Grupos
        public async Task<GrupoDetalleDTO?> GetGrupo()
        {
            var response = await client.GetAsync("api/grupos");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<GrupoDetalleDTO>();
            }

            return null;
        }

        public async Task<AlumnoDetalleDTO?> GetAlumno(int idAlumno)
        {
            var response = await client.GetAsync($"api/grupos/alumno/{idAlumno}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AlumnoDetalleDTO>();
            }

            return null;
        }

        public async Task<bool> EliminarAlumno(int idAlumno)
        {
            var response = await client.DeleteAsync($"api/grupos/{idAlumno}");
            return response.IsSuccessStatusCode;
        }

    }
}
