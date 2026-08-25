using Microsoft.AspNetCore.Mvc;

namespace KitabKlubu.Controllers;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}