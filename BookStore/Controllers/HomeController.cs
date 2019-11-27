using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    [Authorize(AuthenticationSchemes = "Customer")]
    public class HomeController : Controller
    {
        private readonly MyDBContext _ctx;
        public HomeController(MyDBContext myDBContext)
        {
            _ctx = myDBContext;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (TempData["ThongBao"]!=null)
            {
                //if(TempData["ThongBao"].ToString() == "LoginSuccess")
                ViewBag.ThongBaoThanhCong = TempData["ThongBao"].ToString();
            }
            ViewBag.highViewProducts = _ctx.Product.OrderByDescending(p => p.ViewCounts).Take(10).ToList();
            ViewBag.hotViewProducts = _ctx.Product.OrderByDescending(p => p.ViewCounts).OrderByDescending(p=>p.Discount!=0).Take(20).ToList();
            //ViewBag.topSaleProducts = _ctx.Product.OrderByDescending(p => p.ViewCounts).OrderByDescending(p=>p.Discount!=0).Take(10).ToList();
            return View();
        }
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
