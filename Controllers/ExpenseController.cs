using InAndOut.Data;
using InAndOut.Models;
using Microsoft.AspNetCore.Mvc;

namespace InAndOut.Controllers;

public class ExpenseController : Controller
{
    private readonly ApplicationDbContext _context;

    public ExpenseController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        IEnumerable<Expense> objList = _context.Expenses;
        return View(objList);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Expense obj)
    {
        if (ModelState.IsValid)
        {
            _context.Expenses.Add(obj);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    public IActionResult Update(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _context.Expenses.Find(id);
        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(Expense obj)
    {
        if (ModelState.IsValid)
        {
            _context.Expenses.Update(obj);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _context.Expenses.Find(id);
        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        var obj = _context.Expenses.Find(id);
        if (obj == null)
        {
            return NotFound();
        }

        _context.Expenses.Remove(obj);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
