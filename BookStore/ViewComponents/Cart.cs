using BookStore.Helpers;
using BookStore.Models;
using BookStore.ModelViews;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewComponents
{
    public class Cart: ViewComponent
    {
        private readonly MyDBContext _ctx;
        public Cart(MyDBContext myDBContext)
        {
            _ctx = myDBContext;
        }
        public List<CartItem> CartProduct
        {
            get
            {
                var data = HttpContext.Session.GetObject<List<CartItem>>("Cart");
                if (data == null)
                {

                    data = new List<CartItem>();
                }
                return data;
            }
        }
        public IViewComponentResult Invoke()
        {
            var cart = CartProduct;
            return View("Default", cart);
        }
    }
}
