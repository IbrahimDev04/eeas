using ExamHuseyn.Areas.Manage.ViewModels;
using ExamHuseyn.DAL;
using ExamHuseyn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamHuseyn.Areas.Manage.Controllers
{
    [Area("manage")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            if (page <= 0) return BadRequest();
            int count = await _context.Settings.CountAsync();
            if (count <= 0) return NotFound();
            double totalpage = Math.Ceiling((double)count / 3);
            if (page > totalpage) return BadRequest();
            List<Setting> settings = await _context.Settings.Skip((page - 1) * 3).Take(3).ToListAsync();
            if (settings == null) return NotFound();
            PaginationVm<Setting> vm = new PaginationVm<Setting>
            {
                TotalPage = totalpage,
                CurrentPage = page,
                Items = settings
            };
            return View(vm);
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Setting exist = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (exist == null) return NotFound();

            UpdateSettingVm vm = new UpdateSettingVm
            {
                Value = exist.Value,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSettingVm vm)
        {         
            if (!ModelState.IsValid) return View(vm);
            Setting exist = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (exist == null) return NotFound();
            exist.Value = vm.Value;
           
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Setting exist = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (exist == null) return NotFound();
            _context.Settings.Remove(exist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
