using BookStore.Helpers;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class CustomerController : Controller
    {
        private readonly MyDBContext _context;

        public CustomerController(MyDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _context.Customer.ToListAsync();
            return View(model);
        }

        public IActionResult Add()
        {
            Customer customer = new Customer();
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Customer customer, string NameImage, string NameFolder)
        {
            var info = HttpContext.Session.GetObject<Employee>("Employee");

            if (info == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (ModelState.IsValid && customer != null)
            {
                bool check = _context.Customer.Any(c => c.UserName == customer.UserName);
                if (!check)
                {
                    MyTool.MoveImage("Customer", NameImage, NameFolder);
                    Customer cus = _context.Customer.SingleOrDefault(c => c.CustomerId == customer.CustomerId);
                    customer.Image = NameImage;
                    customer.CreatedDate = DateTime.Now;
                    if(cus.Password != customer.Password)
                    {
                        customer.Password = MyHashTool.GetMd5Hash(customer.Password);
                    }
                    _context.Customer.Add(customer);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
            
            var model = await _context.Customer.AsNoTracking().ToListAsync();
            return View("Datatable", model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(e => e.CustomerId == id);

            if (customer == null)
            {
                return BadRequest();
            }

            return PartialView(customer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return BadRequest();
            }

            return View(customer);
        }

        [HttpPost]
        public IActionResult UploadImage([FromForm]IFormFile file)
        {
            string fileName = file.FileName;
            bool check = fileName[0] == 'A'; // check Add or Edit

            string id = "";
            for (int i = 2; i < fileName.Length; ++i)
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
                    var emp = _context.Customer.SingleOrDefault(e => e.CustomerId == int.Parse(id));
                    folder = emp.CustomerId + "_Customer_" + emp.CreatedDate?.ToString("yyyyMMddHHmmssfffffff");
                }
                else
                {
                    folder = "_Customer_" + id;
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

        [HttpPost]
        public async Task<IActionResult> Edit(Customer customer, string NameImage, string NameFolder)
        {
            var info = HttpContext.Session.GetObject<Employee>("Employee");

            if (info == null)
            {
                return RedirectToAction("Index", "Login");
            }
            MyTool.MoveImage("Customer", NameImage, NameFolder);

            var cus = await _context.Customer.AsNoTracking().SingleOrDefaultAsync(e => e.CustomerId == customer.CustomerId);

            if (customer.Password != cus.Password)
            {
                customer.Password = MyHashTool.GetMd5Hash(customer.Password);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customer.Image = NameImage;
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return BadRequest();
                }
                var model = await _context.Customer.AsNoTracking().ToListAsync();
                return View("Datatable", model);
            }
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var info = HttpContext.Session.GetObject<Employee>("Employee");

            if (info == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var customer = await _context.Customer.AsNoTracking().SingleOrDefaultAsync(c => c.CustomerId == id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(c => c.CustomerId == id);
        }

        
    }
}