using AutoMapper;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AvisosAPI.Services
{
    public class AvisosGeneralesService
    {
        private readonly Repository<Avisogeneral> avisoRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly Repository<Alumnoavisogeneral> alumnoAvisoRepository;
        private readonly Repository<Alumno> alumnoRepository;

        public AvisosGeneralesService(
            Repository<Avisogeneral> avisoRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            Repository<Alumnoavisogeneral> alumnoAvisoRepository,
            Repository<Alumno> alumnoRepository)
        {
            this.avisoRepository = avisoRepository;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.alumnoAvisoRepository = alumnoAvisoRepository;
            this.alumnoRepository = alumnoRepository;
        }

        //lista de resumen para alumno
        public List<AvisoGeneralResumenDTO> GetVigentes()
        {
            var idAlumno = ObtenerIdDesdeToken();

            var avisos = alumnoAvisoRepository.Query()
                .Include(x => x.IdAvisoGeneralNavigation)
                .ThenInclude(x => x.IdMaestroNavigation)
                .Include(x => x.IdEstadoNavigation)
                .Where(x => x.IdAlumno == idAlumno 
                         && x.IdAvisoGeneralNavigation.FechaExpira > DateTime.Now 
                         && x.IdAvisoGeneralNavigation.Eliminado == false)
                .OrderByDescending(x => x.IdAvisoGeneralNavigation.FechaEnviado)
                .ToList();

            var avisosMapeados = avisos.Select(x => mapper.Map<AvisoGeneralResumenDTO>(x)).ToList();

            foreach (var a in avisos)
            {
                if (a.IdEstado == 1)
                {
                    a.IdEstado = 2;
                    alumnoAvisoRepository.Update(a);
                }
            }

            return avisosMapeados;
        }

        //lista de resumen para maestro
        public List<AvisoGeneralResumenDTO> GetVigentesMaestro()
        {
            var idMaestro = ObtenerIdDesdeToken();

            var avisos = avisoRepository.Query()
                .Include(x => x.IdMaestroNavigation)
                .Where(x => x.IdMaestro == idMaestro 
                         && x.FechaExpira > DateTime.Now 
                         && x.Eliminado == false)
                .OrderByDescending(x => x.FechaEnviado)
                .ToList();

            var avisosMapeados = avisos.Select(x => mapper.Map<AvisoGeneralResumenDTO>(x)).ToList();

            foreach (var a in avisosMapeados)
            {
                a.IdEstado = null;
                a.NombreEstado = null;
            }

            return avisosMapeados;
        }


        // ver detalle y registrar lectura (alumno)
        public AvisoGeneralDetalleAlumnoDTO GetDetalleAlumno(int idAviso)
        {
            var idAlumno = ObtenerIdDesdeToken();

            var aviso = avisoRepository.Query()
                .Include(x => x.IdMaestroNavigation)
                .FirstOrDefault(x => x.Id == idAviso
                                  && x.FechaExpira > DateTime.Now
                                  && x.Eliminado == false);

            if (aviso == null)
                throw new KeyNotFoundException("Aviso no encontrado o no vigente.");

            // Buscar si ya existe un registro de lectura para este alumno
            var lectura = alumnoAvisoRepository.Query()
                .FirstOrDefault(x => x.IdAvisoGeneral == idAviso
                                  && x.IdAlumno == idAlumno);

            if (lectura == null)
            {
                // Primera vez que el alumno abre este aviso, no deberia pero por si acaso

                alumnoAvisoRepository.Insert(new Alumnoavisogeneral
                {
                    IdAlumno = idAlumno,
                    IdAvisoGeneral = idAviso,
                    IdEstado = 3,           // Leído
                    FechaLeido = DateTime.Now
                });
            }
            else if (lectura.FechaLeido == null)
            {
                lectura.FechaLeido = DateTime.Now;
                lectura.IdEstado = 3; // Leído
                alumnoAvisoRepository.Update(lectura);
            }

            return mapper.Map<AvisoGeneralDetalleAlumnoDTO>(aviso);
        }

        //  ver detalle con listas de lectura
        // Muestra los datos del aviso y separa los alumnos del grupo
        // en dos listas según si ya lo leyeron o no.
        public AvisoGeneralDetalleMaestroDTO GetDetalleMaestro(int idAviso)
        {
            var idMaestro = ObtenerIdDesdeToken();

            var aviso = avisoRepository.Query()
                .Include(x => x.IdMaestroNavigation)
                .FirstOrDefault(x => x.Id == idAviso
                                  && x.IdMaestro == idMaestro
                                  && x.Eliminado == false);

            if (aviso == null)
                throw new KeyNotFoundException("Aviso no encontrado.");

            // Obtener todos los registros de lectura de este aviso
            // incluyendo la navegación al alumno para construir AlumnoLecturaDTO
            var lecturas = alumnoAvisoRepository.Query()
                .Include(x => x.IdAlumnoNavigation)
                .Where(x => x.IdAvisoGeneral == idAviso)
                .ToList();

            var resultado = mapper.Map<AvisoGeneralDetalleMaestroDTO>(aviso);

            // Separar en dos listas según el estado
            resultado.PendientesLectura = lecturas
                .Where(x => x.IdEstado == 1 || x.IdEstado == 2)
                .Select(x => mapper.Map<AlumnoLecturaDTO>(x))
                .ToList();

            resultado.Leidos = lecturas
                .Where(x => x.IdEstado == 3)
                .Select(x => mapper.Map<AlumnoLecturaDTO>(x))
                .ToList();

            return resultado;
        }



        public void Crear(AvisoGeneralCreateDTO dto)
        {

            var idMaestro = ObtenerIdDesdeToken();

            var aviso = mapper.Map<Avisogeneral>(dto);
            aviso.IdMaestro = idMaestro;
            aviso.FechaEnviado = DateTime.Now;
            aviso.Eliminado = false;
            avisoRepository.Insert(aviso);

            var alumnos = alumnoRepository.GetAll().Where(x => x.Eliminado == false).ToList();

            foreach (var alumno in alumnos)
            {
                var alumnoAviso = new Alumnoavisogeneral
                {
                    IdAlumno = alumno.Id,
                    IdAvisoGeneral = aviso.Id,
                    IdEstado= 1 // Nuevo
                    
                };
                alumnoAvisoRepository.Insert(alumnoAviso);
            }

           
        }


        // Verifica que el aviso pertenezca al maestro del token.
        public void Eliminar(int idAviso)
        {
            var idMaestro = ObtenerIdDesdeToken();

            var aviso = avisoRepository.Query()
                .FirstOrDefault(x => x.Id == idAviso
                                  && x.IdMaestro == idMaestro
                                  && x.Eliminado == false);

            if (aviso == null)
                throw new KeyNotFoundException("Aviso no encontrado.");

            aviso.Eliminado = true;
            avisoRepository.Update(aviso);
        }

        private int ObtenerIdDesdeToken()
        {
            var idStr = httpContextAccessor.HttpContext?
                .User.FindFirst("Id")?.Value;

            if (idStr == null)
                throw new UnauthorizedAccessException("No se pudo identificar al usuario.");

            return int.Parse(idStr);
        }
    }
}