using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Areas.ModelViews;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class CommentController : Controller
    {
        private readonly MyDBContext _ctx;
        public CommentController(MyDBContext myDBContext)
        {
            _ctx = myDBContext;
        }
        public IActionResult Index()
        {
            var products = _ctx.Product.Select(p=>new ProductView
            {
                ProductId=p.ProductId,
                ProductName=p.ProductName
            });
            var emp = _ctx.Employee.Select(p => new
            {
                p.EmployeeId,
                Name= p.FirstName + " " + p.LastName
            });
            var cus = _ctx.Customer.Select(p => new
            {
                p.CustomerId,
                Name=p.FirstName+" "+p.LastName
            });
            var comment = _ctx.Comment.Select(p => new CommentViewModel
            {
                CommentId = p.CommentId,
                ProductName = products.SingleOrDefault(pp=>pp.ProductId==p.ProductId).ProductName,
                Name=(p.CustomerId!=null)
                ?(cus.SingleOrDefault(pp=>pp.CustomerId == p.CustomerId).Name)
                :(emp.SingleOrDefault(pp=>pp.EmployeeId == p.EmployeeId).Name)+" (Nhân viên)",
                Context = p.Context,
                CreatedDate = p.CreatedDate,
                ModifiedDate = p.ModifiedDate,
                customerID = p.CustomerId,
                employeeID = p.EmployeeId,
                Status = p.Status
            });
            ViewBag.Products = products;
            return View(comment);
        }

        [HttpPost]
        public int Delete(int id)
        {
            var cmt = _ctx.Comment.SingleOrDefault(p => p.CommentId == id);
            if (cmt != null)
            {
                _ctx.Comment.Remove(cmt);
                _ctx.SaveChanges();
                return 1;
            }
            return 0;
        }

        public IActionResult Filter(int? id)
        {
            if (id == null) return BadRequest();
            var products = _ctx.Product.Select(p => new ProductView
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName
            });
            var emp = _ctx.Employee.Select(p => new
            {
                p.EmployeeId,
                Name = p.FirstName + " " + p.LastName
            });
            var cus = _ctx.Customer.Select(p => new
            {
                p.CustomerId,
                Name = p.FirstName + " " + p.LastName
            });
            if (id != 0)
            {
               var comment = _ctx.Comment.Where(p => p.ProductId == id).Select(p => new CommentViewModel
                {
                    CommentId = p.CommentId,
                    ProductName = products.SingleOrDefault(pp => pp.ProductId == p.ProductId).ProductName,
                    Name = (p.CustomerId != null)
                        ? (cus.SingleOrDefault(pp => pp.CustomerId == p.CustomerId).Name)
                        : (emp.SingleOrDefault(pp => pp.EmployeeId == p.EmployeeId).Name) + " (Nhân viên)",
                    Context = p.Context,
                    CreatedDate = p.CreatedDate,
                    ModifiedDate = p.ModifiedDate,
                    customerID = p.CustomerId,
                    employeeID = p.EmployeeId,
                    Status = p.Status
                });
                return PartialView("Datatable", comment);
            }
            else
            {
                var comment = _ctx.Comment.Select(p => new CommentViewModel
                {
                    CommentId = p.CommentId,
                    ProductName = products.SingleOrDefault(pp => pp.ProductId == p.ProductId).ProductName,
                    Name = (p.CustomerId != null)
                    ? (cus.SingleOrDefault(pp => pp.CustomerId == p.CustomerId).Name)
                    : (emp.SingleOrDefault(pp => pp.EmployeeId == p.EmployeeId).Name) + " (Nhân viên)",
                    Context = p.Context,
                    CreatedDate = p.CreatedDate,
                    ModifiedDate = p.ModifiedDate,
                    customerID = p.CustomerId,
                    employeeID = p.EmployeeId,
                    Status = p.Status
                });
                return PartialView("Datatable", comment);
            }
        }
        public IActionResult Detail(int? id)
        {
            if (id == null) return BadRequest();
            var comment = _ctx.Comment.SingleOrDefault(p => p.CommentId == id);
            if (comment != null)
            {
                string productName = _ctx.Product.SingleOrDefault(p => p.ProductId == comment.ProductId).ProductName;
                string name = "";
                if (comment.EmployeeId != null)
                {
                    //This comment belongs to employee
                    //Getting name of the employee who write this comment
                    var emp = _ctx.Employee.SingleOrDefault(p => p.EmployeeId == comment.EmployeeId);
                    name = emp.FirstName + " " + emp.LastName+" (Nhân viên)";
                }
                else
                {
                    //This comment belongs to customer
                    //Getting name of the customer who write this comment
                    var cus = _ctx.Customer.SingleOrDefault(p => p.CustomerId == comment.CustomerId);
                    name = cus.FirstName + " " + cus.LastName;
                }
                var cmt = new CommentViewModel
                {
                    CommentId = comment.CommentId,
                    ProductName = productName,
                    Name = name,
                    Context = comment.Context,
                    CreatedDate = comment.CreatedDate,
                    ModifiedDate = comment.ModifiedDate,
                    customerID = comment.CustomerId,
                    employeeID = comment.EmployeeId,
                    Status = comment.Status
                };
                return PartialView("Detail", cmt);
            }
            return Content("No available");
            
        }

        [HttpPost]
        public IActionResult Edit(int id, string context, bool status, int isFiltering)
        {
            var comment = _ctx.Comment.SingleOrDefault(p => p.CommentId == id);
            if (comment != null)
            {
                comment.Context = context;
                comment.Status = (status) ? 1 : 0;
                comment.ModifiedDate = DateTime.Now;
                _ctx.Comment.Update(comment);
                _ctx.SaveChanges();
                return Filter(isFiltering);
            }
            return Content("Error");
        }
    }
}