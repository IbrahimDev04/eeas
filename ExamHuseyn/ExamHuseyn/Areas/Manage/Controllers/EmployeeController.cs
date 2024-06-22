using ExamHuseyn.Areas.Manage.ViewModels;
using ExamHuseyn.DAL;
using ExamHuseyn.Models;
using ExamHuseyn.Utilities.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamHuseyn.Areas.Manage.Controllers
{
    [Area("manage")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            if (page <= 0) return BadRequest();
            int count = await _context.Employees.CountAsync();
            if (count <= 0) return NotFound();
            double totalpage = Math.Ceiling((double)count / 3);
            if (page > totalpage) return BadRequest();
            List<Employee> employees = await _context.Employees.Skip((page-1)*3).Take(3).Include(e => e.Position).ToListAsync();
            if (employees == null) return NotFound();
            PaginationVm<Employee> vm = new PaginationVm<Employee>
            {
                TotalPage = totalpage,
                CurrentPage = page,
                Items = employees
            };
            return View(vm);
        }
        public async Task<IActionResult> Create()
        {
            CreateEmpVm vm = new CreateEmpVm
            {
                Positions = await _context.Positions.ToListAsync()
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmpVm vm)
        {
            vm.Positions = await _context.Positions.ToListAsync();
            if(!ModelState.IsValid) return View(vm);
            if(!await _context.Positions.AnyAsync(p => p.Id == vm.PositionId))
            {
                ModelState.AddModelError("PositionId", "The position isn't aviable");
                return View(vm);
            }
            Employee employee = new Employee
            {
                FullName = vm.FullName,
                Fb = vm.Fb,
                Insta = vm.Insta,
                Twitter = vm.Twitter,
                LinkIn = vm.LinkIn,
                PositionId = vm.PositionId,
                Description = vm.Description,
            };
            if (vm.Photo !=null)
            {
                if (vm.Photo.CheckType("image/"))
                {
                    ModelState.AddModelError("Photo", "Photo type incorrect ");
                    return View(vm);
                }
                if (vm.Photo.CheckSize(10))
                {
                    ModelState.AddModelError("Photo", "Photo size incorrect ");
                    return View(vm);
                }
                string filename = await vm.Photo.CreateFileAsync(_env.WebRootPath,"assets","img","team");
                employee.Image = filename;           
            }

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Employee exist = await _context.Employees.Include(e=>e.Position).FirstOrDefaultAsync(e=>e.Id == id);
            if (exist == null) return NotFound();
            UpdateEmpVm vm = new UpdateEmpVm
            {
                FullName = exist.FullName,
                Description = exist.Description,
                Fb = exist.Fb,
                Insta = exist.Insta,
                LinkIn = exist.LinkIn,
                Twitter = exist.Twitter,
                PositionId = exist.PositionId,
                Image = exist.Image,
                Positions = await _context.Positions.ToListAsync()
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateEmpVm vm)
        {
            vm.Positions = await _context.Positions.ToListAsync();
            if (!ModelState.IsValid) return View(vm);
            Employee exist = await _context.Employees.Include(e => e.Position).FirstOrDefaultAsync(e => e.Id == id);
            if (exist == null) return NotFound();
            if (!await _context.Positions.AnyAsync(p => p.Id == vm.PositionId))
            {
                ModelState.AddModelError("PositionId", "The position isn't aviable");
                return View(vm);
            }
            if (vm.Photo != null)
            {
                if (vm.Photo.CheckType("image/"))
                {
                    ModelState.AddModelError("Photo", "Photo type incorrect ");
                    return View(vm);
                }
                if (vm.Photo.CheckSize(10))
                {
                    ModelState.AddModelError("Photo", "Photo size incorrect ");
                    return View(vm);
                }
                exist.Image.DeleteFile(_env.WebRootPath, "assets", "img", "team");
                string filename = await vm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "team");
                exist.Image = filename;               
            }

            exist.FullName = vm.FullName;
            exist.Description = vm.Description;
            exist.PositionId = vm .PositionId;
            exist.Insta= vm.Insta;
            exist.Fb = vm.Fb;
            exist.LinkIn = vm.LinkIn;
            exist.Twitter = vm.Twitter;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Employee exist = await _context.Employees.Include(e => e.Position).FirstOrDefaultAsync(e => e.Id == id);
            if (exist == null) return NotFound();
            exist.Image.DeleteFile(_env.WebRootPath, "assets", "img", "team");
            _context.Employees.Remove(exist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
