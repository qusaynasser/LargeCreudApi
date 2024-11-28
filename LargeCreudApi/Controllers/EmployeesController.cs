using FluentValidation;
using LargeCreudApi.Data;
using LargeCreudApi.DTOs.Employee;
using LargeCreudApi.Model;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace LargeCreudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public EmployeesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync()
        {
            var Employees = await context.Employees.ToListAsync();
            var empDto=Employees.Adapt<IEnumerable<GetEmployeeDto>>();
            return Ok(empDto);
        }

        [HttpGet("Detalis")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var Employee =await context.Employees.FindAsync(id);
            if(Employee == null)
            {
                return NotFound("Employee Not Found");
            }
            var empDto=Employee.Adapt<GetEmployeeDto>();
            return Ok(empDto);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(CreateEmployeeDto empDto,
            [FromServices] IValidator<CreateEmployeeDto> validator)
        {
            var validationResult=validator.Validate(empDto);

            if (!validationResult.IsValid)
            {
                var modelState = new ModelStateDictionary();

                validationResult.Errors.ForEach(error =>
                {
                    modelState.AddModelError(error.PropertyName, error.ErrorMessage);
                });
                return ValidationProblem(modelState);
            }
            var emp=empDto.Adapt<Employee>();
            await context.Employees.AddAsync(emp);
            await context.SaveChangesAsync();

            return Ok();
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(int id,CreateEmployeeDto empDto)
        {
            var Employee =await context.Employees.FindAsync(id);
            if (Employee == null)
            {
                return NotFound("Employee Not Found");
            }
            Employee.Name= empDto.Name;
            Employee.Description= empDto.Description;
            await context.SaveChangesAsync();
            return Ok(Employee);
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            var Employee =await context.Employees.FindAsync(id);
            if (Employee == null)
            {
                return NotFound("Employee Not Found");
            }
            context.Employees.Remove(Employee);
            await context.SaveChangesAsync();
            return Ok("Employee deleted successfully");
        }
    }
}
