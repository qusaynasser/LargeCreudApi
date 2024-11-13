using LargeCreudApi.Data;
using LargeCreudApi.DTOs.Department;
using LargeCreudApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LargeCreudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public DepartmentsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult Get()
        {
            var Departments = context.Departments.Select(
            show =>new GetDepartmentDto()
            {
                Id = show.Id,
                Name = show.Name,
            }
            );
            return Ok(Departments);
        }

        [HttpGet("Detalis")]
        public IActionResult GetById(int id)
        {
            var Department = context.Departments.Find(id);
            
            if (Department == null)
            {
                return NotFound("Department Not Found");
            }
            var dto = new GetDepartmentDto()
            {
                Id=Department.Id,
                Name=Department.Name,
            };
            return Ok(dto);
        }

        [HttpPost("Create")]
        public IActionResult Create(CreateDepartmentDto depDto)
        {
            Department department = new Department()
            {
                Name = depDto.name
            };
            context.Departments.Add(department);
            context.SaveChanges();
            return Ok(department);
        }
        [HttpPut("Update")]
        public IActionResult Update(int id, CreateDepartmentDto department)
        {
            var existingDepartment = context.Departments.Find(id);
            if (existingDepartment == null)
            {
                return NotFound("Department Not Found");
            }

            existingDepartment.Name = department.name;

            context.SaveChanges();
            return Ok(existingDepartment);
        }

        [HttpDelete("Remove")]
        public IActionResult Remove(int id)
        {
            var Department = context.Departments.Find(id);
            if (Department == null)
            {
                return NotFound("Department Not Found");
            }
            context.Departments.Remove(Department);
            context.SaveChanges();
            return Ok("Department deleted successfully");
        }
    }
}
