using FluentValidation;
using LargeCreudApi.DTOs.Register;

namespace LargeCreudApi.DTOs.Account
{
    public class RegisterDtoValidation: AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidation() 
        {
            RuleFor(p=>p.Password).NotEmpty().WithMessage("Pass not empty")
                .Length(5,15).WithMessage("min 5 and max 15");
        }
    }
}
