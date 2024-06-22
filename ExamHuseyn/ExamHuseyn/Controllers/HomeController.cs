using ExamHuseyn.DAL;
using ExamHuseyn.Models;
using ExamHuseyn.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ExamHuseyn.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _context.Employees.Include(e=>e.Position).ToListAsync();
            if(employees==null) return NotFound();
            HomeVm vm = new HomeVm
            {
                Employees = employees
            };
            return View(vm);
        }

      
    }
}