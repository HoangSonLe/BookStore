using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using BookStore.Helpers;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;

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
            if (info.Role == 3) // Role is Employee
            {
                return BadRequest(new { message = "Bạn không được quyền thêm tài khoản!!"});
            }

            Employee emp = new Employee();
            var Roles = _context.Roles.AsNoTracking().Where(r => r.RoleId >= info.Role);
            var Managers = _context.Employee.AsNoTracking().Where(m => m.Role == info.Role).Select(m => new { EmployeeId = m.EmployeeId, Name = m.FirstName + " " + m.LastName});

            ViewData["RoleId"] = new SelectList(Roles, "RoleId", "RoleName", info.Role);
            ViewData["ManageId"] = new SelectList(Managers, "EmployeeId", "Name", info.Role);

            return View(emp);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Employee employee, string NameImage)
        {
            if (ModelState.IsValid && employee != null)
            {
                bool check = _context.Employee.Any(e => e.UserName == employee.UserName);
                
                if (!check)
                {
                    MoveImage(NameImage);
                    employee.Image = NameImage;
                    employee.CreatedDate = DateTime.Now;
                    _context.Employee.Add(employee);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return BadRequest(new { message = "Tài khoản đã tồn tại!!!"});
                }
            }
            else
            {
                return BadRequest(new { message = "Vui lòng điền thông tin tài khoản!"});
            }

            var model = await _context.Employee.AsNoTracking().ToListAsync();
            return View("Datatable", model);
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", id);
            return PartialView(employee);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var employee = await _context.Employee.AsNoTracking().SingleOrDefaultAsync(e => e.EmployeeId == id);
            var info = HttpContext.Session.GetObject<Employee>("Employee");

            if (employee == null)
            {
                return BadRequest();
            }

            if (employee.Role <= info.Role && info.EmployeeId != employee.EmployeeId)
            {
                return BadRequest(new { message = "Bạn không có quyền sửa thông tin người này!!" });
            }

            var Roles = _context.Roles.AsNoTracking().Where(r => r.RoleId >= info.Role);
            var Managers = _context.Employee.AsNoTracking().Where(m => m.Role < employee.Role).Select(m => new { EmployeeId = m.EmployeeId, Name = m.FirstName + " " + m.LastName});

            ViewData["RoleId"] = new SelectList(Roles, "RoleId", "RoleName", employee.Role);
            ViewData["ManageId"] = new SelectList(Managers, "EmployeeId", "Name", employee.Role);

            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee, string NameImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var emp = await _context.Employee.AsNoTracking().SingleOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);

                    if (employee.Password != emp.Password)
                    {
                        employee.Password = MyHashTool.GetMd5Hash(employee.Password);
                    }
                    MoveImage(NameImage);
                    employee.Image = NameImage;
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
            var info = HttpContext.Session.GetObject<Employee>("Employee");
            var employee = await _context.Employee.AsNoTracking().SingleOrDefaultAsync(e => e.EmployeeId == id);

            if (employee.Role <= info.Role)
            {
                return BadRequest( new { message = "Bạn không có quyền xóa người này!!"});
            }
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetManagers(int role)
        {
            var Managers = await _context.Employee.Where(m => m.Role < role).Select(m => new { EmployeeId = m.EmployeeId, Name = m.FirstName + " " + m.LastName }).ToListAsync();

            ViewData["ManageId"] = new SelectList(Managers, "EmployeeId", "Name");
            return View();
        }

        private void MoveImage(string NameImage)
        {
            /*----Start Move file from one folder to another folder*/
            var sourcePath = "wwwroot/img/TmpEmployee/" + NameImage;
            var destinationPath = "wwwroot/img/Employee/" + NameImage;
            if (System.IO.File.Exists(sourcePath))
            {
                System.IO.File.Move(sourcePath, destinationPath);
            }
            /*----End Move file from one folder to another folder*/

            /*----Start Delete file from folder*/
            var path = "wwwroot/img/TmpEmployee/";
            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            /*----End Delete file from folder*/
        }

        [HttpPost]
        public IActionResult UploadImage([FromForm]IFormFile file)
        {
            if (file != null)
            {
                var name = MyTool.UploadHinh(file, "TmpEmployee");
                return Ok(new { name = name });
            }
            return Ok();
        }
    }
}