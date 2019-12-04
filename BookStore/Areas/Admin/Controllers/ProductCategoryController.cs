using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var category = _ctx.ProductCategory.AsNoTracking().ToList();
            return View(category);
        }
        
        public IActionResult CreateOrEdit(int? id)
        {
            var category = _ctx.ProductCategory.Where(p => p.ParentId == null).ToList();
            ViewBag.category = category;
            if (id != null)
            {
                ViewBag.Action = "edit";
                var productCategory = new ProductCategory();

                if (id != null)
                {
                    productCategory = _ctx.ProductCategory.Where(p => p.CategoryId == id).SingleOrDefault();
                }
                return PartialView("CreateOrEdit", productCategory);
            }
            ViewBag.Action = "add";
            return PartialView("CreateOrEdit");
        }

        [HttpPost]
        public IActionResult CreateOrEdit(ProductCategory productCategory)
        {
            if (productCategory.CategoryId == 0)
            {
                //ADD
                var parentID = (productCategory.ParentId == 0) ? null : productCategory.ParentId;
                ProductCategory category = new ProductCategory
                {
                    Name = productCategory.Name,
                    UrlFriendly = productCategory.UrlFriendly,
                    ParentId = parentID,
                    Status = (productCategory.Status != null) ? true : false
                };
                _ctx.ProductCategory.Add(category);
                _ctx.SaveChanges();
            }
            else
            {
                //EDIT
                var category = _ctx.ProductCategory.Where(p => p.CategoryId == productCategory.CategoryId).SingleOrDefault();
                if (category != null)
                {
                    var parentID = (productCategory.ParentId == 0) ? null : productCategory.ParentId;
                    category.Name = productCategory.Name;
                    category.UrlFriendly = productCategory.UrlFriendly;
                    category.ParentId = parentID;
                    category.Status = (productCategory.Status != null) ? true : false;
                    _ctx.ProductCategory.Update(category);
                    _ctx.SaveChanges();
                }
            }
            var listCategories = _ctx.ProductCategory.ToList();
            return PartialView("Reload", listCategories);
        }

        [HttpPost]
        public int Delete(int? id)
        {
            try
            {
                //in case of delete foreign key
                var item = _ctx.ProductCategory.Where(p => p.CategoryId == id).SingleOrDefault();
                _ctx.ProductCategory.Remove(item);
                _ctx.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
              
            }
            return 0;
        }

        public string GenerateSlug(string phrase)
        {
            string str = RemoveDiacritics(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public string RemoveDiacritics( string text)
        {
            var s = new string(text.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());

            return s.Normalize(NormalizationForm.FormC);
        } 
    }

}
