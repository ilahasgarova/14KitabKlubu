using Microsoft.AspNetCore.Mvc;
using KitabKlubu.Models;

namespace KitabKlubu.Controllers;

public class ArticlesController : Controller
{
    private readonly AppDbContext _context;

    public ArticlesController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View(_context.Articles.ToList());
    }

    public IActionResult Detail(int id)
    {
        var article = _context.Articles.FirstOrDefault(a => a.Id == id);
        if (article == null)
        {
            return NotFound();
        }
        return View(article);
    }
}