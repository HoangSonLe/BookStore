using System;
using System.Linq;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    [Authorize(AuthenticationSchemes = "Admin")]
    [Route("Admin")]
    public class ChartController : Controller
    {
        private readonly MyDBContext _ctx;
        public ChartController(MyDBContext myDBContext)
        {
            _ctx = myDBContext;
        }
        [Route("Chart/Order")]
        public IActionResult Order()
        {
            ModelViews.OrderViewModel data = new ModelViews.OrderViewModel(){ StartDate = "",EndDate="", TypeChart="line"};
            return View(data);
        }
        [HttpPost]
        [Route("Chart/Order")]
        public IActionResult Order([Bind("startDate")] DateTime startDate, [Bind("endDate")] DateTime endDate,[Bind("typeChart")] string typeChart)
        {
            var chart = _ctx.Orders.Where(p => p.CreatedDate < endDate && p.CreatedDate > startDate)
                                  .GroupBy(p => new
                                  {
                                      Nam = p.CreatedDate.Value.Year,
                                      Thang = p.CreatedDate.Value.Month
                                  })
                                  .OrderBy(p => p.Key.Nam)
                                  .ThenBy(p=> p.Key.Thang)
                                  .Select(p => new ModelViews.OrderItemViewModel
                                  {
                                      Nam = p.Key.Nam,
                                      Thang = p.Key.Thang,
                                      Tong = p.Count()
                                  })
                                  .ToList();
            ModelViews.OrderViewModel data = new ModelViews.OrderViewModel
            {
                StartDate = startDate.ToShortDateString(),
                EndDate = endDate.ToShortDateString(),
                TypeChart= typeChart,
                orderItems = chart
            };
            return View(data);
        }
    }
}