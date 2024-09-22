using InAndOut.Data;
using InAndOut.Models;
using Microsoft.AspNetCore.Mvc;

namespace InAndOut.Controllers;

public class ItemController : Controller
{
    private readonly ApplicationDbContext _context;

    public ItemController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        IEnumerable<Item> objList = _context.Items;
        return View(objList);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Item obj)
    {
        _context.Items.Add(obj);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
