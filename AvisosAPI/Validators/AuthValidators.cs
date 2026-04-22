using FluentValidation;
using static AvisosAPI.Models.DTOs.AuthDTOs;

namespace AvisosAPI.Validators
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.NumControl)
                .NotEmpty().WithMessage("El número de control es requerido.")
                .Must(x => x.Length == 4 || x.Length == 8).WithMessage("El número de control es requerido.");

            RuleFor(x => x.Contrasena)
                .NotEmpty().WithMessage("La contraseña es requerida.");
        }
    }
}
