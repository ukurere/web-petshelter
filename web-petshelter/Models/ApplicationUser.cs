using Microsoft.AspNetCore.Identity;

namespace web_petshelter.Models;

public class ApplicationUser : IdentityUser
{
    public bool IsAdmin { get; set; }
}
