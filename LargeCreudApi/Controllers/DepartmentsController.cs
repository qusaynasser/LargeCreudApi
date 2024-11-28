using LargeCreudApi.Data;
using LargeCreudApi.DTOs.Department;
using LargeCreudApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [Authorize(AuthenticationSchemes ="Bearer")]
        public async Task<IActionResult> GetAsync()
        {
            var Departments = await context.Departments.Select(
            show =>new GetDepartmentDto()
            {
                Id = show.Id,
                Name = show.Name,
            }
            ).ToListAsync();
            return Ok(Departments);
        }

        [HttpGet("Detalis")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var Department =await context.Departments.FindAsync(id);
            
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
        public async Task<IActionResult> CreateAsync(CreateDepartmentDto depDto)
        {
            Department department = new Department()
            {
                Name = depDto.name
            };
            await context.Departments.AddAsync(department);
           await context.SaveChangesAsync();
            return Ok(department);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(int id, CreateDepartmentDto department)
        {
            var existingDepartment =await context.Departments.FindAsync(id);
            if (existingDepartment == null)
            {
                return NotFound("Department Not Found");
            }

            existingDepartment.Name = department.name;

            await context.SaveChangesAsync();
            return Ok(existingDepartment);
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            var Department =await context.Departments.FindAsync(id);
            if (Department == null)
            {
                return NotFound("Department Not Found");
            }
            context.Departments.Remove(Department);
            await context.SaveChangesAsync();
            return Ok("Department deleted successfully");
        }
    }
}
