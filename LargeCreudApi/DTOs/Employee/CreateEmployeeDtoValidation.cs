using FluentValidation;

namespace LargeCreudApi.DTOs.Employee
{
    public class CreateEmployeeDtoValidation:AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeDtoValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("name is emptay!!");
            RuleFor(x => x.Description).MinimumLength(10).WithMessage("description lenght error");
            RuleFor(x => x.DepartmentId).GreaterThan(0).WithMessage("dep id invalid");
        }
    }
}
