using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KitabKlubu.Models;

namespace KitabKlubu.Controllers;

[Authorize]
public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Dashboard()
    {
        var kitablar = _context.Books.ToList();
        var meqaleler = _context.Articles.ToList();

        ViewBag.CemiKitab = kitablar.Count;
        ViewBag.TesdiqlenmisKitab = kitablar.Count(b => b.IsApproved);
        ViewBag.GozleyenKitab = kitablar.Count(b => !b.IsApproved);
        ViewBag.CemiMeqale = meqaleler.Count;

        return View();
    }

    public IActionResult Index()
    {
        var kitablar = _context.Books.OrderByDescending(b => b.Id).ToList();
        return View(kitablar);
    }

    [HttpGet]
    public IActionResult AddBook()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddBook(Book book)
    {
        book.IsApproved = true;
        book.SubmittedBy = string.IsNullOrWhiteSpace(book.SubmittedBy) ? "Admin" : book.SubmittedBy;
        book.DateAdded = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        _context.Books.Add(book);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var kitab = _context.Books.FirstOrDefault(b => b.Id == id);
        if (kitab == null)
        {
            return NotFound();
        }
        return View(kitab);
    }

    [HttpPost]
    public IActionResult Edit(Book book)
    {
        var mevcud = _context.Books.FirstOrDefault(b => b.Id == book.Id);
        if (mevcud != null)
        {
            mevcud.Title = book.Title;
            mevcud.Author = book.Author;
            mevcud.Price = book.Price;
            mevcud.Description = book.Description;
            mevcud.ImageUrl = book.ImageUrl;
            mevcud.SubmittedBy = book.SubmittedBy;
            mevcud.ContactInfo = book.ContactInfo;
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    public IActionResult Approve(int id)
    {
        var kitab = _context.Books.FirstOrDefault(b => b.Id == id);
        if (kitab != null)
        {
            kitab.IsApproved = true;
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    public IActionResult Reject(int id)
    {
        var kitab = _context.Books.FirstOrDefault(b => b.Id == id);
        if (kitab != null)
        {
            kitab.IsApproved = false;
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var kitab = _context.Books.FirstOrDefault(b => b.Id == id);
        if (kitab != null)
        {
            _context.Books.Remove(kitab);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    public IActionResult Articles()
    {
        var meqaleler = _context.Articles.OrderByDescending(a => a.Id).ToList();
        return View(meqaleler);
    }

    [HttpGet]
    public IActionResult AddArticle()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddArticle(Article article)
    {
        article.Date = DateTime.Now.ToString("dd.MM.yyyy");
        _context.Articles.Add(article);
        _context.SaveChanges();
        return RedirectToAction("Articles");
    }

    [HttpGet]
    public IActionResult EditArticle(int id)
    {
        var meqale = _context.Articles.FirstOrDefault(a => a.Id == id);
        if (meqale == null)
        {
            return NotFound();
        }
        return View(meqale);
    }

    [HttpPost]
    public IActionResult EditArticle(Article article)
    {
        var mevcud = _context.Articles.FirstOrDefault(a => a.Id == article.Id);
        if (mevcud != null)
        {
            mevcud.Icon = article.Icon;
            mevcud.Title = article.Title;
            mevcud.Summary = article.Summary;
            mevcud.Content = article.Content;
            _context.SaveChanges();
        }
        return RedirectToAction("Articles");
    }

    public IActionResult DeleteArticle(int id)
    {
        var meqale = _context.Articles.FirstOrDefault(a => a.Id == id);
        if (meqale != null)
        {
            _context.Articles.Remove(meqale);
            _context.SaveChanges();
        }
        return RedirectToAction("Articles");
    }
}