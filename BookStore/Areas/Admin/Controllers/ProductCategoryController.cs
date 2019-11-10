using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize]
    [Authorize(AuthenticationSchemes = "Admin")]

    public class ProductCategoryController : Controller
    {
        private readonly MyDBContext _ctx;
        public ProductCategoryController(MyDBContext myDBContext)
        {
            _ctx = myDBContext;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "Product Category";
            var category = _ctx.ProductCategory.ToList();
            return View(category);
        }
        
        public IActionResult CreateOrEdit(int? id)
        {
            var productCategory =new ProductCategory();
            var category = _ctx.ProductCategory.Where(p => p.ParentId == null).ToList();
            ViewBag.category = category;
            if (id!= null)
            {
                productCategory = _ctx.ProductCategory.Where(p => p.CategoryId == id).SingleOrDefault();
            }
            return PartialView("CreateOrEdit",productCategory);
        }
        [HttpPost]
        public IActionResult CreateOrEdit(int? id, IFormFile data)
        {
            return Ok();
        }
        
        [HttpPost]
        public int Delete(int? id)
        {
            //var item = _ctx.Publishers.SingleOrDefault(p => p.PublisherId == id);
            //if (item !=null)
            //{
            //    return 1;
            //}
            //else
            //{
            //    return 0;
            //}
            return 1;
        }

    }
}