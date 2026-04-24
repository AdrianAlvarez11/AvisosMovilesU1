using FluentValidation;
using AvisosAPI.Models.DTOs;
using System;

namespace AvisosAPI.Validators
{
    public class AvisoGeneralCreateValidator : AbstractValidator<AvisoGeneralCreateDTO>
    {
        public AvisoGeneralCreateValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("El título es requerido.")
                .MaximumLength(50).WithMessage("El título no debe exceder los 50 caracteres.");
                
            RuleFor(x => x.Contenido)
                .NotEmpty().WithMessage("El contenido no puede estar vacío.");
                
            RuleFor(x => x.FechaExpira)
                .GreaterThan(DateTime.UtcNow).WithMessage("La fecha de expiración debe ser en el futuro.");
        }
    }

    public class AvisoPersonalCreateValidator : AbstractValidator<AvisoPersonalCreateDTO>
    {
        public AvisoPersonalCreateValidator()
        {
            RuleFor(x => x.IdAlumno)
                .GreaterThan(0).WithMessage("El ID del alumno es inválido.");
                
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("El título es requerido.")
                .MaximumLength(50).WithMessage("El título no debe exceder los 50 caracteres.");
                
            RuleFor(x => x.Contenido)
                .NotEmpty().WithMessage("El contenido no puede estar vacío.");
        }
    }
}
