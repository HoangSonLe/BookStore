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
using System.Collections.Generic;

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

            if (info == null)
            {
                return BadRequest(new { message = "login" });
            }

            if (info.Role == 3) // Role is Employee
            {
                return BadRequest(new { content = "Bạn không được quyền thêm tài khoản!!"});
            }

            Employee emp = new Employee();
            var Roles = _context.Roles.AsNoTracking().Where(r => r.RoleId >= info.Role);
            var Managers = _context.Employee.AsNoTracking().Where(m => m.Role == info.Role).Select(m => new { EmployeeId = m.EmployeeId, Name = m.FirstName + " " + m.LastName});

            ViewData["RoleId"] = new SelectList(Roles, "RoleId", "RoleName", info.Role);
            ViewData["ManageId"] = new SelectList(Managers, "EmployeeId", "Name", info.Role);

            return PartialView(emp);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Employee employee, string NameImage, string NameFolder)
        {
            var info = HttpContext.Session.GetObject<Employee>("Employee");

            if (info == null)
            {
                return BadRequest(new { message = "login" });
            }
            if (ModelState.IsValid && employee != null)
            {
                bool check = _context.Employee.Any(e => e.UserName == employee.UserName);   
                if (!check)
                {
                    MyTool.MoveImage("Employee", NameImage, NameFolder);
                    employee.Image = NameImage;
                    employee.CreatedDate = DateTime.Now;
                    employee.Password = MyHashTool.GetMd5Hash(employee.Password);
                   
                    _context.Employee.Add(employee);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return BadRequest(new {content = "Tài khoản đã tồn tại!!!"});
                }
            }
            else
            {
                return BadRequest(new { content = "Vui lòng điền thông tin tài khoản!"});
            }

            var model = await _context.Employee.AsNoTracking().ToListAsync();
            return View("Datatable", model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var info = HttpContext.Session.GetObject<Employee>("Employee");

            if (info == null)
            {
                return BadRequest(new { message = "login" });
            }
            if (id == null)
            {
                return BadRequest();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return BadRequest();
            }
            ViewBag.Role = _context.Roles
                .FirstOrDefault(e => e.RoleId == employee.Role).RoleName;
            var manager = _context.Employee.FirstOrDefault(e => e.EmployeeId == employee.ManagerId);
            if(manager != null)
            {
                ViewBag.Manager = manager.FirstName + " " + manager.LastName;
            }
            else
            {
                ViewBag.Manager = "";
            }

            return PartialView(employee);
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            var info = HttpContext.Session.GetObject<Employee>("Employee");
            // xét còn session
            if (info == null)
            {
                return BadRequest(new { message = "login" });
            }

            var employee = await _context.Employee.AsNoTracking().SingleOrDefaultAsync(e => e.EmployeeId == id);
            
            if (employee == null)
            {
                return BadRequest(new { content = "Không tồn tại nhân viên này!!" });
            }

            if (employee.Role <= info.Role && info.EmployeeId != employee.EmployeeId)
            {
                return BadRequest(new { content = "Bạn không có quyền sửa thông tin người này!!" });
            }

            var Roles = _context.Roles.AsNoTracking().Where(r => r.RoleId >= info.Role);
            
            if (employee.EmployeeId == info.EmployeeId)
            {
                var Managers = _context.Employee.AsNoTracking().Where(m => m.EmployeeId == employee.ManagerId).Select(m => new { EmployeeId = m.EmployeeId, Name = m.FirstName + " " + m.LastName });

                ViewData["ManageId"] = new SelectList(Managers, "EmployeeId", "Name", employee.Role);
            }
            else
            {
                var Managers = _context.Employee.AsNoTracking().Where(m => m.Role < employee.Role).Select(m => new { EmployeeId = m.EmployeeId, Name = m.FirstName + " " + m.LastName });
                ViewData["ManageId"] = new SelectList(Managers, "EmployeeId", "Name", employee.Role);
            }

            ViewData["RoleId"] = new SelectList(Roles, "RoleId", "RoleName", employee.Role);
            
            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("EmployeeId,UserName,Password,FirstName,LastName,Address,Email,Sex,Phone,BirthDate,Role,ManagerId,Image,IsActive")] Employee employee, string NameImage, string NameFolder)
        {
            var info = HttpContext.Session.GetObject<Employee>("Employee");

            if (info == null)
            {
                return BadRequest(new { message = "login" });
            }
            

            if (ModelState.IsValid)
            {
                try
                {
                    var emp = await _context.Employee.AsNoTracking().SingleOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);
                    if (emp == null)
                    {
                        return BadRequest(new { content = "Không tồn tại nhân viên này!!" });
                    }
                    if (employee.Password != emp.Password)
                    {
                        employee.Password = MyHashTool.GetMd5Hash(employee.Password);
                    }
                    MyTool.MoveImage("Employee", NameImage, NameFolder);
                    employee.Image = NameImage;

                    employee.UserName = emp.UserName;
                    _context.Update(employee);
                    await _context.SaveChangesAsync();

                }
                catch
                {
                    return BadRequest(new { content = "Sửa không thành công!" });
                }
            }

            var model = await _context.Employee.AsNoTracking().ToListAsync();
            return View("Datatable", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var info = HttpContext.Session.GetObject<Employee>("Employee");

            if (info == null)
            {
                return BadRequest(new { message = "login" });
            }
            if (!EmployeeExists(id))
            {
                return BadRequest(new { content = "Không tồn tại nhân viên này!!" });
            }

            var employee = await _context.Employee.AsNoTracking().SingleOrDefaultAsync(e => e.EmployeeId == id);

            if (employee.Role <= info.Role)
            {
                return BadRequest( new { content = "Bạn không có quyền xóa người này!!"});
            }
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            var model = await _context.Employee.AsNoTracking().ToListAsync();
            return View("Datatable", model);
        }

        [HttpPost]
        public async Task<IActionResult> GetManagers(int role, int? idEmployee)
        {
            var info = HttpContext.Session.GetObject<Employee>("Employee");

            if (info == null)
            {
                return BadRequest(new { message = "login" });
            }

            var employee = _context.Employee.SingleOrDefault(e => e.EmployeeId == idEmployee);

            if (employee.Role == role)
            {
                var Managers = _context.Employee.AsNoTracking().Where(m => m.EmployeeId == employee.ManagerId).Select(m => new { EmployeeId = m.EmployeeId, Name = m.FirstName + " " + m.LastName });

                ViewData["ManageId"] = new SelectList(Managers, "EmployeeId", "Name", employee.Role);
            }
            else
            {
                var Managers = await _context.Employee.Where(m => m.Role < role && m.EmployeeId != idEmployee).Select(m => new { EmployeeId = m.EmployeeId, Name = m.FirstName + " " + m.LastName }).ToListAsync();
                ViewData["ManageId"] = new SelectList(Managers, "EmployeeId", "Name");
            }

            return View();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }

        [HttpPost]
        public IActionResult UploadImage([FromForm]IFormFile file)
        {
            var info = HttpContext.Session.GetObject<Employee>("Employee");

            if (info == null)
            {
                return BadRequest(new { message = "login" });
            }

            string fileName = file.FileName;
            bool check = fileName[0] == 'A'; // check Add or Edit

            string id = "";
            for(int i=2; i<fileName.Length; ++i)
            {
                if (fileName[i] != '_')
                {
                    id += fileName[i];
                }
                else
                {
                    break;
                }
            }



            if (file != null)
            {
                string folder = "";
                if (!check)
                {
                    var emp = _context.Employee.SingleOrDefault(e => e.EmployeeId == int.Parse(id));
                    folder = emp.EmployeeId + "_Employee_" + emp.CreatedDate?.ToString("yyyyMMddHHmmss");
                }
                else
                {
                    folder = "_Employee_" + id;
                }
                var pathString = "wwwroot/Image/" + folder;
                Directory.CreateDirectory(pathString);
                var name = MyTool.UploadHinh(file, folder);
                return Ok(new { name = name, folder = folder });
            }
            return BadRequest();
        }

        [HttpPost]
        public void DeleteFolderTmp(string NameFolder)
        {
            MyTool.DeleteFolder(NameFolder);
        }
    }
}