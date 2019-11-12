using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using BookStore.Helpers;
using System;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin")]

    public class EmployeeController : Controller
    {
        private readonly MyDBContext _context;

        public EmployeeController(MyDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _context.Employee.ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var info = HttpContext.Session.GetObject<Employee>("Employee");
            Employee emp = new Employee();
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", info.Role);
            var managers = _context.Employee.Where(p => p.Role < info.Role).ToList();

            ViewBag.Managers = managers;
            return View(emp);
        }
        [HttpPost]
        public async Task<IActionResult> Add(Employee employee)
        {
            bool check = _context.Employee.Any(e => e.EmployeeId == employee.EmployeeId);
            employee.CreatedDate = DateTime.Now;
            if (!check)
            {
                _context.Employee.Add(employee);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var employee = await _context.Employee
                .Include(e => e.RoleNavigation)
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return BadRequest();
            }

            return PartialView(employee);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return BadRequest();
            }

            var info = HttpContext.Session.GetObject<Employee>("Employee");
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", 1);
            var managers = _context.Employee.Where(p => p.Role < 1).ToList();

            ViewBag.Managers = managers;
            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            var emp = await _context.Employee.AsNoTracking().SingleOrDefaultAsync(e=>e.EmployeeId == employee.EmployeeId);

            if (employee.Password != emp.Password)
            {
                employee.Password = MyHashTool.GetMd5Hash(employee.Password);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return BadRequest();
                }
                return RedirectToAction(nameof(Index));
            }
            var info = HttpContext.Session.GetObject<Employee>("Employee");
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", info.Role);
            var managers = _context.Employee.Where(p => p.Role < info.Role).ToList();

            ViewBag.Managers = managers;
            return View(employee);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var employee = await _context.Employee.AsNoTracking().SingleOrDefaultAsync(e => e.EmployeeId == id);
            var info = HttpContext.Session.GetObject<Employee>("Employee");
            if (employee.Role <= info.Role)
            {
                return BadRequest();
            }
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}