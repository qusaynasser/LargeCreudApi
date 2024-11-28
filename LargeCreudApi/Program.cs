
using FluentValidation;
using LargeCreudApi.Data;
using LargeCreudApi.DTOs.Account;
using LargeCreudApi.DTOs.Employee;
using LargeCreudApi.DTOs.Register;
using LargeCreudApi.Errors;
using LargeCreudApi.Identity;
using LargeCreudApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Microsoft.Extensions.Logging;

namespace LargeCreudApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection"));
            });

            builder.Services.AddScoped<IValidator<CreateEmployeeDto>, CreateEmployeeDtoValidation>();
            builder.Services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidation>();
            builder.Services.AddScoped<AuthServices>();

            builder.Services.AddAuthentication("Bearer").AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer=true,
                    ValidateAudience=true,
                    ValidIssuer=builder.Configuration.GetSection("jwt:issuer").Value,
                    ValidAudience=builder.Configuration.GetSection("jwt:audience").Value,
                    ValidateLifetime=true,
                    IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("jwt")["secretKey"])),

                };
            });
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.Services.AddIdentity<AppUser, IdentityRole>(options=>
            {
                options.Password.RequireDigit = false; // ?? ???? ???????
                options.Password.RequireUppercase = false; // ?? ???? ?????? ???????
                options.Password.RequireLowercase = false; // ?? ???? ?????? ???????
                options.Password.RequireNonAlphanumeric = false; // ?? ???? ?????? ??????
                options.Password.RequiredLength = 6; // ???? ?????? ?????
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
            });

            builder.Host.UseSerilog((cotext, configuration) =>
            {
                configuration.ReadFrom.Configuration(cotext.Configuration);
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");
            app.UseAuthorization();


            app.MapControllers();

            app.UseExceptionHandler(opt => { });
            app.Run();
        }
    }
}
