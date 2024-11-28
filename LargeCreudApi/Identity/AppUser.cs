using LargeCreudApi.Data;
using Microsoft.AspNetCore.Identity;

namespace LargeCreudApi.Identity
{
    public class AppUser: IdentityUser
    {
        public string DisplayName { get; set; }
        public string? Address { get; set; }
    }
}
