using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Helpers;
using BookStore.Models;
using BookStore.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    [Authorize(AuthenticationSchemes = "Customer")]
    public class CartController : Controller
    {
        private readonly MyDBContext _ctx;
        public CartController(MyDBContext myDBContext)
        {
            _ctx = myDBContext;
        }
        
        public List<CartItem> Cart
        {
            get{
                var data = HttpContext.Session.GetObject<List<CartItem>>("Cart");
                if (data == null)
                {

                    data = new List<CartItem>();
                }
                else
                {
                    foreach(var d in data)
                    {
                        var p = _ctx.Product.Find(d.ProductId);
                        if(p== null)
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
            List<CartItem> cart = Cart; 
            return View(cart);
        }
        [AllowAnonymous]
        public IActionResult AddToCart(int id, int quantity)
        {
            try
            {
                List<CartItem> cartItems = Cart;
                CartItem cartItem = cartItems.SingleOrDefault(o => o.ProductId == id);
                
                if (cartItem == null)
                {
                    Product p = _ctx.Product.Find(id);
                    if (p != null)
                    {
                        cartItems.Add(new CartItem
                        {
                            ProductId = p.ProductId,
                            ProductName = p.ProductName,
                            Price = (p.Discount == 0) ? p.Price : p.PromotionPrice,
                            ImageCover = p.ImageCover,
                            QuantityProduct = quantity,
                        });
                    }
                }
                else
                {
                    cartItem.QuantityProduct += quantity;
                }
                HttpContext.Session.SetObject<List<CartItem>>("Cart", cartItems);
                ViewBag.Success = "success";
            }
            catch (Exception)
            {
                ViewBag.Success = "error";
            }
            List<CartItem> cart = HttpContext.Session.GetObject<List<CartItem>>("Cart");
            return PartialView("PopupCart", cart);
        }
        [AllowAnonymous]
        public IActionResult UpdateToCart(int id, int quantity)
        {
            try
            {
                List<CartItem> cartItems = Cart;
                CartItem cartItem = cartItems.SingleOrDefault(o => o.ProductId == id);
                cartItem.QuantityProduct += quantity;
                HttpContext.Session.SetObject<List<CartItem>>("Cart", cartItems);
                ViewBag.Success = "success";
            }
            catch (Exception)
            {
                ViewBag.Success = "error";
            }
            List<CartItem> cart = HttpContext.Session.GetObject<List<CartItem>>("Cart");
            return PartialView("ProductCart", cart);
        }
        [AllowAnonymous]
        public IActionResult UpdateNumberToCart(int id, int quantity)
        {
            try
            {
                List<CartItem> cartItems = Cart;
                CartItem cartItem = cartItems.SingleOrDefault(o => o.ProductId == id);
                cartItem.QuantityProduct = quantity;
                HttpContext.Session.SetObject<List<CartItem>>("Cart", cartItems);
                ViewBag.Success = "success";
            }
            catch (Exception)
            {
                ViewBag.Success = "error";
            }
            List<CartItem> cart = HttpContext.Session.GetObject<List<CartItem>>("Cart");
            return PartialView("ProductCart", cart);
        }
        [AllowAnonymous]
        public IActionResult DeleteInCart(int id)
        {
            List<CartItem> cartItems = Cart;
            CartItem cartItem = cartItems.SingleOrDefault(o => o.ProductId == id);
            cartItems.Remove(cartItem);
            HttpContext.Session.SetObject<List<CartItem>>("Cart", cartItems);
            List<CartItem> cart = HttpContext.Session.GetObject<List<CartItem>>("Cart");
            return PartialView("ProductCart", cart);
        }
        [AllowAnonymous]
        public IActionResult ViewProductCart()
        {
            List<CartItem> cart = HttpContext.Session.GetObject<List<CartItem>>("Cart");
            return PartialView("PopupCart", cart);
        }
    }
}