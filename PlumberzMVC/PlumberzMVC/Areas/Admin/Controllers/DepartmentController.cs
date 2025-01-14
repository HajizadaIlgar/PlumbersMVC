using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlumberzMVC.Contexts;
using PlumberzMVC.Models;
using PlumberzMVC.ViewModels;

namespace PlumberzMVC.Areas.Admin.Controllers;
[Area(nameof(Admin))]
public class DepartmentController(PlumbersDbContext _context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var data = await _context.Departments.ToListAsync();
        return View(data);
    }
    public ActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DepartmentVM vm)
    {
        Department dp = new Department
        {
            DepartmentName = vm.DepartmentName,
        };
        await _context.Departments.AddAsync(dp);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Update(int? id)
    {
        if (id == null) return BadRequest();
        var data = await _context.Departments.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (data is null) return NotFound();
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int? id, Department item)
    {
        var data = await _context.Departments.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (data is not null)
        {
            data.DepartmentName = item.DepartmentName;
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id)
    {
        var data = await _context.Departments.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (data is null) return NotFound();
        _context.Departments.Remove(data);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
