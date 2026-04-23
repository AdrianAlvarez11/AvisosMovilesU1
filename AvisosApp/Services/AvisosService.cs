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
        string baseUrl = "http://localhost:5058/";
        HttpClient client;

        public AvisosService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        // AUTH
        public async Task<LoginResponseDTO?> Login(LoginDTO dto)
        {
            var response = await client.PostAsJsonAsync("api/auth", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
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

        //Avisos Personales
        public async Task<List<AvisoPersonalResumenDTO>> GetMisAvisos()
        {
            var response = await client.GetAsync("api/avisospersonales");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AvisoPersonalResumenDTO>>() ?? [];
            }

            return [];
        }

        public async Task<AvisoPersonalDetalleDTO?> GetDetalle(int idAviso)
        {
            var response = await client.GetAsync($"api/avisospersonales/{idAviso}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AvisoPersonalDetalleDTO>();
            }

            return null;
        }

        public async Task<bool> Crear(AvisoPersonalCreateDTO dto)
        {
            var response = await client.PostAsJsonAsync("api/avisospersonales", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<AvisoPersonalDetalleDTO>> GetDeAlumno(int idAlumno)
        {
            var response = await client.GetAsync($"api/avisospersonales/alumno/{idAlumno}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AvisoPersonalDetalleDTO>>() ?? [];
            }

            return [];
        }

        public async Task<bool> Eliminar(int idAviso)
        {
            var response = await client.DeleteAsync($"api/avisospersonales/{idAviso}");
            return response.IsSuccessStatusCode;
        }

        // Avisos Grupales
        public async Task<List<AvisoGeneralResumenDTO>> GetAvisos()
        {
            var response = await client.GetAsync("api/avisosgenerales");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AvisoGeneralResumenDTO>>() ?? [];
            }

            return [];
        }

        public async Task<AlumnoDetalleDTO?> GetDetalleAlumno(int idAviso)
        {
            var response = await client.GetAsync($"api/avisosgenerales/alumno/{idAviso}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AlumnoDetalleDTO>();
            }

            return null;
        }

        public async Task<AvisoGeneralDetalleMaestroDTO?> GetDetalleMaestro(int idAviso)
        {
            var response = await client.GetAsync($"api/avisosgenerales/maestro/{idAviso}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AvisoGeneralDetalleMaestroDTO>();
            }

            return null;
        }

        public async Task<bool> Crear(AvisoGeneralCreateDTO dto)
        {
            var response = await client.PostAsJsonAsync("api/avisosgenerales", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarAvisoGeneral(int idAviso)
        {
            var response = await client.DeleteAsync($"api/avisosgenerales/{idAviso}");
            return response.IsSuccessStatusCode;
        }

    }
}
