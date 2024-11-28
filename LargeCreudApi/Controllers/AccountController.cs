using FluentValidation;
using LargeCreudApi.Data;
using LargeCreudApi.DTOs.Account;
using LargeCreudApi.DTOs.Register;
using LargeCreudApi.Identity;
using LargeCreudApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace LargeCreudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly AuthServices authServices;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,AuthServices authServices)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.authServices = authServices;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto Vmodel, [FromServices] IValidator<RegisterDto> validator)
        {
            var validationResult = validator.Validate(Vmodel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(error => new
                {
                    Field = error.PropertyName,
                    Error = error.ErrorMessage
                }));
            }

            var user = new AppUser()
            {
                DisplayName = Vmodel.Name,
                Email = Vmodel.Email,
                PhoneNumber = Vmodel.PhoneNumber,
                UserName = Vmodel.Email.Split('@')[0]
            };

            var result = await userManager.CreateAsync(user, Vmodel.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "success" });
        }


        [HttpPost("Login")]
        
        public async Task<IActionResult> Login(LoginDto Vmodel)
        {
           
            // البحث عن المستخدم بواسطة البريد الإلكتروني
            var user = await userManager.FindByEmailAsync(Vmodel.Email);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            // التحقق من صحة كلمة المرور
            var result = await signInManager.CheckPasswordSignInAsync(user, Vmodel.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            // إرجاع بيانات المستخدم في حالة النجاح
            return Ok(new UserDto()
            {
                Name = user.DisplayName,
                Email = user.Email,
                Token=await authServices.CreateTokenAsync(user,userManager)
            });
        }

    }
}
