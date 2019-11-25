using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;
using BookStore.Helpers;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    [Authorize(AuthenticationSchemes = "Customer")]
    public class ProductsController : Controller
    {
        private readonly MyDBContext _context;
        public ProductsController(MyDBContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        // GET: Products
        public async Task<IActionResult> Index(string sortOrder,int? pageNumber, string currentCategory)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ViewSortParm"] = sortOrder == "View" ? "view_desc" : "View";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            ViewData["CurrentCategory"] = !String.IsNullOrEmpty(currentCategory) ? currentCategory : "";
            var myDBContext = _context.Product.Include(p => p.Category).Include(p => p.Publisher).AsQueryable();
            switch (sortOrder)
            {
                case "name_desc":
                    myDBContext = myDBContext.OrderByDescending(s => s.ProductName);
                    break;
                case "View":
                    myDBContext = myDBContext.OrderBy(s => s.ViewCounts);
                    break;
                case "view_desc":
                    myDBContext = myDBContext.OrderByDescending(s => s.ViewCounts);
                    break;
                case "Price":
                    myDBContext = myDBContext.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    myDBContext = myDBContext.OrderByDescending(s => s.Price);
                    break;
                default:
                    myDBContext = myDBContext.OrderBy(s => s.ProductName);
                    break;
            }
            int pageSize = 12;
            if(currentCategory != null)
            {
                myDBContext = myDBContext.Where(p => p.Category.UrlFriendly == currentCategory);
            }

            //ViewBag.Category = from c in _context.ProductCategory where (c.UrlFriendly == currentCategory) select c.CategoryId; ;
            ViewData["Publisher"] = _context.Publishers.ToList();
            return View(await myDBContext.ToList().ToPagedListAsync(pageNumber ?? 1, pageSize));
        }

        [AllowAnonymous]
        // GET: Products/Details/5
        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Product
                .Include(p => p.Category)
                .Include(p => p.Publisher)
                .Include(p=>p.ProductImages)
                .SingleOrDefault(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return PartialView("ModalDetail",product);
        }

        [AllowAnonymous]
        [Route("{loai}/{hanghoa}")]
        public IActionResult DetailPage(string loai, string hanghoa)
        {
            var category = _context.ProductCategory.SingleOrDefault(p => p.UrlFriendly == loai);
            if (category == null) return BadRequest();
            else
            {
                int CateID = category.CategoryId;
                var product = _context.Product
                            .Include(x => x.ProductImages)
                            .Include(x => x.Publisher)
                            .Where(p => p.CategoryId == CateID && p.UrlFriendly == hanghoa)
                            .AsNoTracking()
                            .SingleOrDefault();
                var relativeProduct = _context.Product
                                     .Include(x => x.Category)
                                     .Where(p => p.CategoryId == CateID || p.PublisherId == product.PublisherId)
                                     .AsNoTracking()
                                     .OrderByDescending(x=>x.ProductId)
                                     .Take(6);
                var newProducts = _context.Product
                                .Include(x => x.Category)
                                .AsNoTracking()
                                .OrderByDescending(x => x.ProductId)
                                .Take(6);
                ViewBag.relative = relativeProduct;
                ViewBag.newProducts = newProducts;
                return View(product);
            }
        }

        [AllowAnonymous]
        public IActionResult GetUpSellPRoducts(int category, int exceptProID)
        {
            var products = _context.Product
                                    .Include(x => x.Category)
                                    .Where(p => p.CategoryId == category && p.ProductId != exceptProID)
                                    .ToList();
            return PartialView("UpsellProducts",products);
        }

        [AllowAnonymous]
        public IActionResult GetNewProducts()
        {
            var products = _context.Product.OrderByDescending(x => x.ProductId).Take(6);
            return PartialView("NewProducts");
        }

    }
}
