using FluentValidation;
using static AvisosAPI.Models.DTOs.RegistroDTOs;
using AvisosAPI.Repositories;
using AvisosAPI.Models.Entities;
using System.Linq;

namespace AvisosAPI.Validators
{
    public class AlumnoRegistroValidator : AbstractValidator<AlumnoRegistroDTO>
    {
        public AlumnoRegistroValidator(Repository<Alumno> alumnoRepository)
        {
            RuleFor(x => x.NumControl)
                .NotEmpty().WithMessage("El número de control es requerido.")
                .Matches("(?i)^[0-9]{2}1[AGDTPMQV][ED0-9][0-9]{3}$").WithMessage("El número de control no tiene un formato válido.")
                .Must(numControl => !alumnoRepository.Query().Any(a => a.NumControl == numControl))
                .WithMessage("El número de control ya está registrado en el sistema.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es requerido.");
                
            RuleFor(x => x.Contrasena)
                .NotEmpty().WithMessage("La contraseña es requerida.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }
    }

    public class MaestroRegistroValidator : AbstractValidator<MaestroRegistroDTO>
    {
        public MaestroRegistroValidator(Repository<Maestro> maestroRepository)
        {
            RuleFor(x => x.NumControl)
                .NotEmpty().WithMessage("El número de control es requerido.")
                .Matches("^[0-9]{4}$").WithMessage("El número de control no tiene un formato válido.")
                .Must(numControl => !maestroRepository.Query().Any(m => m.NumControl == numControl))
                .WithMessage("El número de control ya está registrado en el sistema.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es requerido.");
                
            RuleFor(x => x.Contrasena)
                .NotEmpty().WithMessage("La contraseña es requerida.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
                
            RuleFor(x => x.NombreGrupo)
                .NotEmpty().WithMessage("El nombre del grupo es requerido.");
        }
    }
}
