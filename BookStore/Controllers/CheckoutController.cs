using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Areas.ModelViews;
using BookStore.Helpers;
using BookStore.Models;
using BookStore.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    [Authorize(AuthenticationSchemes = "Customer")]
    public class CheckoutController : Controller
    {
        private readonly MyDBContext _context;
        public CheckoutController(MyDBContext myDBContext)
        {
            _context = myDBContext;
        }

        public List<CartItem> Cart
        {
            get
            {
                var data = HttpContext.Session.GetObject<List<CartItem>>("Cart");
                if (data == null)
                {

                    data = new List<CartItem>();
                }
                else
                {
                    foreach (var d in data)
                    {
                        var p = _context.Product.AsNoTracking().SingleOrDefault(o => o.ProductId == d.ProductId);
                        if (p == null)
                        {
                            data.Remove(d);
                        }
                        else
                        {
                            d.Price = (p.Discount == 0) ? p.Price : p.PromotionPrice;
                        }
                    }
                }
                return data;
            }
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if(Cart.Count() == 0)
            {
                return RedirectToAction("EmptyCart");
            }
            Orders order = new Orders();
            var info = HttpContext.Session.GetObject<Customer>("Customer");
            if (info != null)
            {
                order.Name = info.LastName + " " + info.FirstName;
                order.Address = info.Address;
                order.Phone = info.PhoneNumber;
                order.Email = info.Email;
            }

            var CartItems = HttpContext.Session.GetObject<List<CartItem>>("Cart");
            ViewBag.cartItems = CartItems as List<CartItem>;
            return View(order);
        }

        [AllowAnonymous]
        public IActionResult Invoice(Orders order)
        {
            if (Cart.Count() == 0)
            {
                return RedirectToAction("EmptyCart");
            }
            var info = HttpContext.Session.GetObject<Customer>("Customer");
            if (info != null)
            {
                order.CustomerId = info.CustomerId;
            }
            if (order.PayMethod == "DirectPay")
            {
                DirectPay(order, Cart);
            }
            ViewBag.cartItems = Cart as List<CartItem>;
            HttpContext.Session.Remove("Cart");
            return View(order);
        }

        [AllowAnonymous]
        public IActionResult PaymentFailed()
        {
            var model = _context.Product.Include(p => p.Category)
                                               .OrderByDescending(p => p.ProductId)
                                               .Take(10)
                                               .AsNoTracking()
                                               .Where(p => p.Status == true)
                                               .ToList();
            return View(model);
        }

        public void DirectPay(Orders order, List<CartItem> CartItems)
        {
            order.Total = Cart.Sum(p => p.Price * p.QuantityProduct);
            if (order.Total < 100000)
            {
                order.ShipCost = 20000;
            }
            else if (order.Total < 500000)
            {
                order.ShipCost = 12000;
            }
            else
            {
                order.ShipCost = 0;
            }
            order.CreatedDate = DateTime.Now;
            order.OrderStatus = 0;
            _context.Orders.Add(order);
            _context.SaveChanges();
            var IdLasted = order.OrderId;
            foreach (var item in Cart)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderId = IdLasted,
                    ProductId = item.ProductId,
                    Price = item.Price,
                    Quantity = item.QuantityProduct
                };
                _context.OrderDetail.Add(orderDetail);
                _context.SaveChanges();
            }
        }
        [AllowAnonymous]
        public IActionResult EmptyCart()
        {

            var model = _context.Product.Include(p => p.Category)
                                               .OrderByDescending(p => p.ProductId)
                                               .Take(10)
                                               .AsNoTracking()
                                               .Where(p => p.Status == true)
                                               .ToList();
            return View(model);
        }

    }
}