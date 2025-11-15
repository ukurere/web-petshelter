using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using web_petshelter.Models;

namespace web_petshelter.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LogoutModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LogoutModel(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    // Якщо хтось відкриє /Identity/Account/Logout GET-ом — просто розлогінюємо й кидаємо на головну
    public async Task<IActionResult> OnGet()
    {
        if (User?.Identity?.IsAuthenticated == true)
        {
            await _signInManager.SignOutAsync();
        }

        return Redirect("~/");
    }

    // Наші кнопки Logout у шапці роблять POST — логіка та сама
    public async Task<IActionResult> OnPost()
    {
        if (User?.Identity?.IsAuthenticated == true)
        {
            await _signInManager.SignOutAsync();
        }

        return Redirect("~/");
    }
}
