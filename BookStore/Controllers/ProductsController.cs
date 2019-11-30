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
using BookStore.ModelViews;

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
        [Route("{currentCategory}")]
        public async Task<IActionResult> Index(string sortOrder,int? pageNumber, string currentCategory)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ViewSortParm"] = sortOrder == "View" ? "view_desc" : "View";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            ViewData["CurrentCategory"] = !String.IsNullOrEmpty(currentCategory) ? currentCategory : "";
            var myDBContext = _context.Product.Include(p => p.Category).Include(p => p.Publisher).Where(p => p.Status == true).AsQueryable();
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
                myDBContext = myDBContext.Where(p => p.Category.UrlFriendly == currentCategory).Where(p => p.Status == true).AsNoTracking();
            }

            //ViewBag.Category = from c in _context.ProductCategory where (c.UrlFriendly == currentCategory) select c.CategoryId; ;
            ViewData["Publisher"] = _context.Publishers.ToList();
            return View(await myDBContext.ToList().ToPagedListAsync(pageNumber ?? 1, pageSize));
        }

        [AllowAnonymous]
        // GET: Products/Details/5
        [Route("chi-tiet/{id}")]
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
                .AsNoTracking()
                .SingleOrDefault(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return PartialView("ModalDetail",product);
        }

        [AllowAnonymous]
        [Route("{loai}/{hanghoa}")]
        public async Task<IActionResult> DetailPage(string loai, string hanghoa)
        {
            var category = _context.ProductCategory.AsNoTracking().SingleOrDefault(p => p.UrlFriendly == loai);
            if (category == null) return BadRequest();
            else
            {
                int CateID = category.CategoryId;
                var product = _context.Product
                            .Include(x => x.ProductImages)
                            .Include(x => x.Publisher)
                            .Include(x=>x.Category)
                            .Where(p => p.CategoryId == CateID && p.UrlFriendly == hanghoa && p.Status==true)
                            .AsNoTracking()
                            .SingleOrDefault();
                if (product == null) return BadRequest();
                //Getting relative products based on publisher and category except it
                var relativeProduct = await _context.Product
                                     .Include(x => x.Category)
                                     .Where(p => p.ProductId != product.ProductId && (p.CategoryId == CateID || p.PublisherId == product.PublisherId))
                                     .Where(p => p.Status == true)
                                     .AsNoTracking()
                                     .OrderByDescending(x=>x.ProductId)
                                     .Take(6)
                                     .ToListAsync();
                var newProducts = await _context.Product
                                .Include(x => x.Category)
                                .Where(p => p.Status == true)
                                .AsNoTracking()
                                .OrderByDescending(x => x.ProductId)
                                .Take(6)
                                .ToListAsync();
                ViewBag.relativeProducts = relativeProduct;
                ViewBag.newProducts = newProducts;
                return View(product);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public void IncreaseViewCount(int productID)
        {
            var product = _context.Product.SingleOrDefault(p => p.ProductId == productID);
            //Thêm asnoTacking nó sẽ ko savechange đc
            //save change ko có thay đổi
                
            if (product != null)
            {
                product.ViewCounts++;
                _context.Product.Update(product);
                _context.SaveChanges();
            }
        }

        [AllowAnonymous]
        public IActionResult GetComments(int productID)
        {
            var comment = _context.Comment
                        .AsNoTracking()
                        .Include(x => x.Customer)
                        .Include(x => x.Employee)
                        .Where(p => p.ProductId == productID && p.Status == 1)
                        .Select(p => new CommentView
                        {
                            ID = p.CommentId,
                            Name = (p.Employee != null) ?
                                   (p.Employee.FirstName + " " + p.Employee.LastName + " (Nhân Viên)") :
                                   (p.Customer.FirstName + " " + p.Customer.LastName),
                            Image = (p.Employee != null) ?
                                    p.Employee.Image :
                                    p.Customer.Image,
                            Context = p.Context,
                            CustomerID = p.CustomerId,
                            EmployeeID = p.EmployeeId,
                            TimeAgo=TimeAgo(p.CreatedDate.Value)
                        });
            return PartialView("Comments", comment);
                            
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddComment(int ProductID, string Context)
        {

            //Checking Session whether it is expired
            if (HttpContext.Session.GetObject<Customer>("Customer") != null || HttpContext.Session.GetObject<Employee>("Employee") != null)
            {
                Customer cus = HttpContext.Session.GetObject<Customer>("Customer");
                Employee emp = HttpContext.Session.GetObject<Employee>("Employee");
                Comment comment = new Comment
                {
                    ProductId = ProductID,
                    Context = Context,
                    CreatedDate = DateTime.Now,
                    Status = 1
                };
                if (cus != null)
                { 
                    comment.CustomerId = cus.CustomerId;
                }
                else
                {
                    comment.EmployeeId = emp.EmployeeId;
                }
                _context.Comment.Add(comment);
                _context.SaveChanges();
                
                return GetComments(ProductID);
            }
            
            return Content("error");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult EditComment(int CommentID, int ProductID, string Context)
        {
            var comment = _context.Comment.SingleOrDefault(p => p.CommentId == CommentID);
            if (comment != null)
            {
                comment.Context = Context;
                comment.ModifiedDate = DateTime.Now;
                _context.Comment.Update(comment);
                _context.SaveChanges();
                return GetComments(ProductID);
            }
            return Content("error");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult DeleteComment(int CommentID, int ProductID)
        {
            var comment = _context.Comment.SingleOrDefault(p => p.CommentId == CommentID);
            if (comment != null)
            {
                _context.Remove(comment);
                _context.SaveChanges();
                return GetComments(ProductID);
            }
            return Content("error");
        }

        public string TimeAgo(DateTime dt)
        {
            if (dt > DateTime.Now)
                return "about sometime from now";
            TimeSpan span = DateTime.Now - dt;

            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("{0} năm trước", years);
            }

            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("{0} tháng trước", months);
            }

            if (span.Days > 0)
                return String.Format("{0} ngày trước", span.Days);

            if (span.Hours > 0)
                return String.Format("{0} giờ trước", span.Hours);

            if (span.Minutes > 0)
                return String.Format("{0} phút trước", span.Minutes);

            if (span.Seconds > 5)
                return String.Format("{0} giây trước", span.Seconds);

            if (span.Seconds <= 5)
                return "Mới đây";

            return string.Empty;
        }

    }
}
