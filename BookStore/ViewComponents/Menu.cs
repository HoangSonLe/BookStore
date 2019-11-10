using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewComponents
{
    public class Menu: ViewComponent
    {
        private readonly MyDBContext _ctx;
        public Menu(MyDBContext myDBContext)
        {
            _ctx = myDBContext;
        }
        public IViewComponentResult Invoke()
        {
            var menu = _ctx.ProductCategory.ToList();
            return View("Default", menu);
        }
    }
}
