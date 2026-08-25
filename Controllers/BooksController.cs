using Microsoft.AspNetCore.Mvc;
using KitabKlubu.Models;

namespace KitabKlubu.Controllers;

public class BooksController : Controller
{
    private readonly AppDbContext _context;

    public BooksController(AppDbContext context)
    {
        _context = context;
    }
    public IActionResult Index(string? axtaris, int page = 1)
    {
        const int SehifeOlcusu = 6;

        var kitablar = _context.Books.Where(b => b.IsApproved).AsQueryable();

        if (!string.IsNullOrWhiteSpace(axtaris))
        {
            kitablar = kitablar.Where(b => b.Title.Contains(axtaris) || b.Author.Contains(axtaris));
        }

        var cemiKitab = kitablar.Count();
        var cemiSehife = (int)Math.Ceiling(cemiKitab / (double)SehifeOlcusu);
        if (cemiSehife < 1) cemiSehife = 1;

        if (page < 1) page = 1;
        if (page > cemiSehife) page = cemiSehife;

        var netice = kitablar
            .OrderByDescending(b => b.Id)
            .Skip((page - 1) * SehifeOlcusu)
            .Take(SehifeOlcusu)
            .ToList();

        ViewBag.Axtaris = axtaris;
        ViewBag.CariSehife = page;
        ViewBag.CemiSehife = cemiSehife;

        return View(netice);
    }
    public IActionResult Detail(int id)
    {
        var kitab = _context.Books.FirstOrDefault(b => b.Id == id && b.IsApproved);
        if (kitab == null)
        {
            return NotFound();
        }
        return View(kitab);
    }

    [HttpGet]
    public IActionResult Submit()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Submit(Book book)
    {
        book.IsApproved = false;
        book.DateAdded = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        _context.Books.Add(book);
        _context.SaveChanges();

        ViewBag.Ugurlu = true;
        return View();
    }
}