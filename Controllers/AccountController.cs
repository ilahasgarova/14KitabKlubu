using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using KitabKlubu.Models;
using System.Security.Claims;

namespace KitabKlubu.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string istifadeciAdi, string parol)
    {
        var hash = PasswordHelper.Hash(parol);
        var admin = _context.AdminUsers.FirstOrDefault(a => a.Username == istifadeciAdi && a.PasswordHash == hash);

        if (admin != null)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, istifadeciAdi) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            return RedirectToAction("Dashboard", "Admin");
        }

        ViewBag.Xeta = "İstifadəçi adı və ya parol yanlışdır.";
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}