using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            Orders order = new Orders();
            return View(order);
        }
    }
}