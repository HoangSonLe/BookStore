using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class FeedbackController : Controller
    {

        private readonly MyDBContext _ctx;
        public FeedbackController(MyDBContext myDBContext)
        {
            _ctx = myDBContext;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "Feedback";
            var listFB = _ctx.Feedback.ToList();
            return View(listFB);
        }

        [HttpPost]
        public int Delete(int id)
        {
            var fb = _ctx.Feedback.Where(p => p.FeedbackId == id).SingleOrDefault();
            if (fb != null)
            {
                _ctx.Feedback.Remove(fb);
                _ctx.SaveChanges();
                return 1;
            }
            return 0;
        }

        public IActionResult Detail(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var fb = _ctx.Feedback.Where(p => p.FeedbackId == id).SingleOrDefault();
            if (fb == null) return BadRequest(); 
            if (fb.EmployeeId != null)
            {
                //Feedback need to reply from staff and Someone already replied
                var emp = _ctx.Employee.SingleOrDefault(p => p.EmployeeId == id);
                ViewBag.EmployeeName = emp.FirstName + " " + emp.LastName;
            }
            return PartialView("Detail", fb);
        }
    }
}