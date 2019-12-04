using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin")]

    public class RolesController : Controller
    {
        private readonly MyDBContext _context;

        public RolesController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Admin/Roles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.AsNoTracking().ToListAsync());
        }

        public IActionResult Create()
        {
            return PartialView();
        }


       
        [HttpPost]
        public IActionResult Create([Bind("RoleName")] Roles roles)
        {
            _context.Roles.Add(roles);
            _context.SaveChanges();
            var r = _context.Roles.AsNoTracking();
            return PartialView("Datatable", r);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roles = await _context.Roles.FindAsync(id);
            if (roles == null)
            {
                return NotFound();
            }
            return PartialView(roles);
        }


        [HttpPost]
        public IActionResult Edit(int id, string name)
        {
            var r = _context.Roles.SingleOrDefault(p => p.RoleId == id);
            if (r != null)
            {
                r.RoleName = name;
                _context.Roles.Update(r);
                _context.SaveChanges();
                var roles = _context.Roles.AsNoTracking();
                return PartialView("Datatable",roles);
            }
            return BadRequest();
        }

 
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var roles = _context.Roles.Find(id);
            _context.Roles.Remove(roles);
            _context.SaveChanges();
            return this.Ok();
        }

       
    }
}
