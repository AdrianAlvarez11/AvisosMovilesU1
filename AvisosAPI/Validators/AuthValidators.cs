using FluentValidation;
using static AvisosAPI.Models.DTOs.AuthDTOs;

namespace AvisosAPI.Validators
{
    public class AlumnoLoginValidator : AbstractValidator<AlumnoLoginDTO>
    {
        public AlumnoLoginValidator()
        {
            RuleFor(x => x.NumControl)
                .NotEmpty().WithMessage("El número de control es requerido.");
            
            RuleFor(x => x.Contrasena)
                .NotEmpty().WithMessage("La contraseña es requerida.");
        }
    }

    public class MaestroLoginValidator : AbstractValidator<MaestroLoginDTO>
    {
        public MaestroLoginValidator()
        {
            RuleFor(x => x.NumControl)
                .NotEmpty().WithMessage("El número de control es requerido.");
                
            RuleFor(x => x.Contrasena)
                .NotEmpty().WithMessage("La contraseña es requerida.");
        }
    }
}
