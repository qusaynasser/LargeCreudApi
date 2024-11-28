using LargeCreudApi.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LargeCreudApi.Services
{
    public class AuthServices
    {
        private readonly IConfiguration configuration;

        public AuthServices(IConfiguration configuration) 
        {
            this.configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManager)
        {
            var authClamis = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),

            };
            var userRoles=await userManager.GetRolesAsync(user);
            foreach(var role in userRoles)
            {
                authClamis.Add(new Claim(ClaimTypes.Role,role));
            }

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("jwt")["secretKey"]));

            var token = new JwtSecurityToken(
                
                claims:authClamis,
                signingCredentials:new SigningCredentials(authKey,SecurityAlgorithms.HmacSha256Signature),
                expires: DateTime.UtcNow.AddDays(1),
                audience:"Qshop users", // مين رح يستخدم هاض التوكن
                issuer:"Qshop App" // مين عمل هاض التوكن عادة بكون اسم المشروع
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
