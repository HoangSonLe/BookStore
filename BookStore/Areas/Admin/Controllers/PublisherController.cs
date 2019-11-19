using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin")]

    public class PublisherController : Controller
    {
        private readonly MyDBContext _ctx;
        public PublisherController (MyDBContext myDBContext)
        {
            _ctx = myDBContext;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "Publishers";
            var publishers = _ctx.Publishers.ToList();
            return View(publishers);
        }
        public int Delete(int id)
        {
            var publisher = _ctx.Publishers.SingleOrDefault(p => p.PublisherId == id);
            if (publisher != null)
            {
                _ctx.Publishers.Remove(publisher);
                _ctx.SaveChanges();
                return 1;
            }
            return 0;

        }
        public IActionResult Edit(int? id)
        {
            var pub = _ctx.Publishers.SingleOrDefault(p => p.PublisherId == id);
            return PartialView("Edit",pub);
        }
        [HttpPost]
        public IActionResult UploadFile(IFormFile img)
        {
           
                if (img != null)
                {
                    string fileName = $"{DateTime.Now.Ticks}{img.FileName}";
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Admin" ,"Image", fileName);

                    using (var f = new FileStream(fullPath, FileMode.Create))
                    {
                        img.CopyTo(f);
                    }
                    
                }

            return Content("Uploaded");
        }
    }
}