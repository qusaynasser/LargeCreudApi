using LargeCreudApi.Data;
using LargeCreudApi.DTOs.Employee;
using LargeCreudApi.Model;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Get()
        {
            var Employees=context.Employees.ToList();
            var empDto=Employees.Adapt<IEnumerable<GetEmployeeDto>>();
            return Ok(empDto);
        }

        [HttpGet("Detalis")]
        public IActionResult GetById(int id)
        {
            var Employee = context.Employees.Find(id);
            if(Employee == null)
            {
                return NotFound("Employee Not Found");
            }
            var empDto=Employee.Adapt<GetEmployeeDto>();
            return Ok(empDto);
        }

        [HttpPost("Create")]
        public IActionResult Create(CreateEmployeeDto empDto)
        {
            var emp=empDto.Adapt<Employee>();
            context.Employees.Add(emp);
            context.SaveChanges();

            return Ok();
        }
        [HttpPut("Update")]
        public IActionResult Update(int id,CreateEmployeeDto empDto)
        {
            var Employee = context.Employees.Find(id);
            if (Employee == null)
            {
                return NotFound("Employee Not Found");
            }
            Employee.Name= empDto.Name;
            Employee.Description= empDto.Description;
            context.SaveChanges();
            return Ok(Employee);
        }

        [HttpDelete("Remove")]
        public IActionResult Remove(int id)
        {
            var Employee = context.Employees.Find(id);
            if (Employee == null)
            {
                return NotFound("Employee Not Found");
            }
            context.Employees.Remove(Employee);
            context.SaveChanges();
            return Ok("Employee deleted successfully");
        }
    }
}
