using Microsoft.AspNetCore.Identity;

namespace TodoApi.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
