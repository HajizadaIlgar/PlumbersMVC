using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlumberzMVC.Contexts;
using PlumberzMVC.Extensions;
using PlumberzMVC.Models;
using PlumberzMVC.ViewModels;

namespace PlumberzMVC.Areas.Admin.Controllers;
[Area(nameof(Admin))]
public class WorkerController : Controller
{
    public readonly PlumbersDbContext _context;
    public readonly IWebHostEnvironment _env;
    public WorkerController(PlumbersDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
    public async Task<IActionResult> Index()
    {
        var data = await _context.Workers.Include(x => x.Department).ToListAsync();
        return View(data);
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.Department = await _context.Departments.Where(x => !x.IsDeleted).ToListAsync();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(WorkerVM vm)
    {

        if (vm.Image != null)
        {
            if (!vm.Image.IsValidType("image"))
                ModelState.AddModelError("File", "File must be an image");
            if (!vm.Image.IsValidSize(400))
                ModelState.AddModelError("File", "File must be less than 400kb");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _context.Departments.Where(x => !x.IsDeleted).ToListAsync();
            return View(vm);
        }

        if (!await _context.Departments.AnyAsync(x => x.Id == vm.DepartmentId && !x.IsDeleted))
        {
            ViewBag.Categories = await _context.Departments.Where(x => !x.IsDeleted).ToListAsync();
            ModelState.AddModelError("DepartmentId", "Department not found or deleted");
            return View(vm);
        }

        var productImagePath = Path.Combine(_env.WebRootPath, "imgs", "workers");
        Worker worker = new Worker
        {
            FullName = vm.FullName,
            Designation = vm.Designation,
            ImageUrl = productImagePath,
            DepartmentId = vm.DepartmentId
        };
        if (vm.Image != null)
        {
            worker.ImageUrl = await vm.Image.UploadAsync(productImagePath);
        }

        await _context.Workers.AddAsync(worker);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Update(int? id)
    {
        if (id == null) return BadRequest();
        var data = await _context.Workers.Where(x => x.Id == id).Select(x => new WorkerUpdateVM
        {
            FullName = x.FullName,
            Designation = x.Designation,
            DepartmentId = x.DepartmentId ?? 0,
            FileUrl = x.ImageUrl,
        }).FirstOrDefaultAsync();
        if (data == null) return NotFound();
        ViewBag.Department = await _context.Departments.Where(x => !x.IsDeleted).ToListAsync();
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int? id, WorkerUpdateVM vm)
    {
        if (id == null) return BadRequest();
        var data = await _context.Workers.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (data == null) return NotFound();
        if (vm.Image is not null)
        {
            vm.Id = data.Id;
            vm.FullName = data.FullName;
            vm.Designation = data.Designation;
            vm.DepartmentId = data.DepartmentId ?? 0;
            vm.FileUrl = data.ImageUrl;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return BadRequest();
        var data = await _context.Workers.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (data == null) return NotFound();
        _context.Workers.Remove(data);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
