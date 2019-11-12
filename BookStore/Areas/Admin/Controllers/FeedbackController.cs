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
            _ctx.Feedback.Remove(fb);
            _ctx.SaveChanges();
            return 1;
        }
    }
}