using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlumberzMVC.Contexts;
using PlumberzMVC.ViewModels;

namespace PlumberzMVC.Controllers
{
    public class HomeController(PlumbersDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Workers.Include(x => x.Department).Where(x => !x.IsDeleted).Select(x => new WorkerListItemVm
            {
                FullName = x.FullName,
                ImageUrl = x.ImageUrl,
                DepartmentId = x.DepartmentId!.Value,
                Designation = x.Designation,
            }).ToListAsync());
        }
    }
}
