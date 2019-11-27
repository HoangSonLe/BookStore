using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ProductsController : Controller
    {
        private readonly MyDBContext _context;

        public ProductsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.Product.Include(p => p.Category).Include(p => p.Publisher);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Publisher)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return PartialView(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.ProductCategory.Where(p => p.ParentId != null).ToList(), "CategoryId", "Name");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName");

            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ffile")] IFormFile Ffile,[Bind(" ProductName,Unit,UrlFriendly,Description,Price,PromotionPrice,IncludeVat,Quantity,CategoryId,PublisherId,Discount,ViewCounts,Status")] Product product, List<IFormFile> fFiles)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //thêm ảnh bìa
                    product.ImageCover = UploadAnhBia(Ffile);
                    //Thêm product vào db
                    _context.Add(product);
                    _context.SaveChanges();
                    //thêm hình ảnh mô tả
                    UploadAnhMoTa(fFiles, product.ProductId);
                    ViewBag.Message = "success";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.ProductCategory, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName", product.PublisherId);
            return View();
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(m => m.ProductImages).AsNoTracking().SingleOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            var ProductImages = new List<ProductImages>();
            ProductImages = _context.ProductImages.Where(p => p.ProductId == id).ToList();
            ViewBag.ProductImages = ProductImages;
            ViewData["CategoryId"] = new SelectList(_context.ProductCategory.Where(p=>p.ParentId!=null).ToList(), "CategoryId", "Name", product.CategoryId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName", product.PublisherId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("Ffile")] IFormFile Ffile, [Bind("ArrDeleteImage")] string ArrDeleteImage, [Bind("ProductId,ProductName,Unit,UrlFriendly,Description,Price,PromotionPrice,IncludeVat,Quantity,CategoryId,PublisherId,Discount,ViewCounts,Status")] Product product, List<IFormFile> fFiles)
        {
            //mảng id hình ảnh xóa
            string[] arrDeleteImage = new string[] { };

            if (id != product.ProductId)
            {
                return NotFound();
            }
            if (ArrDeleteImage.Length > 0)
            {
                ArrDeleteImage = ArrDeleteImage.Trim();
                arrDeleteImage = ArrDeleteImage.Split(',');
            }
            if (ModelState.IsValid)
            {
                try
                {
                    //thêm hình ảnh mô tả
                    UploadAnhMoTa(fFiles, id);
                    //xóa hình ảnh mô tả
                    if (arrDeleteImage.Count() > 0)
                    {
                        foreach (var o in arrDeleteImage)
                        {
                            try
                            {
                                var image = _context.ProductImages.AsNoTracking().SingleOrDefault(p => p.ProductImagesId == int.Parse(o.Trim()));
                                if (image != null)
                                {
                                    XoaAnh(image.ProductImage);
                                    _context.ProductImages.Remove(image);
                                }
                            }
                            catch (Exception) { }

                        }
                    }
                    //thêm và xóa ảnh bìa
                    var productBefore = _context.Product.AsNoTracking().SingleOrDefault(p => p.ProductId == id);
                    if (Ffile!=null && Ffile.Length != 0)
                    {
                        if (productBefore.ImageCover != null)
                        {
                            XoaAnh(product.ImageCover);
                        }
                        //Thêm Ảnh Bìa
                        product.ImageCover = UploadAnhBia(Ffile);
                    }
                    else
                    {
                        product.ImageCover = productBefore.ImageCover;
                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    ViewBag.Message = "success";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            var ProductImages = new List<ProductImages>();
            ProductImages = _context.ProductImages.Where(p => p.ProductId == id).ToList();
            ViewBag.ProductImages = ProductImages;
            ViewData["CategoryId"] = new SelectList(_context.ProductCategory, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName", product.PublisherId);
            return View(product);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _context.Product.AsNoTracking().SingleOrDefault(p=>p.ProductId==id);
            var images = _context.ProductImages.AsNoTracking().Where(p => p.ProductId == id).ToList();

            if (product != null)
            {
                try
                {
                    //Xóa hình mô tả
                    if (images.Count() > 0)
                    {
                        foreach (var image in images)
                        {
                            XoaAnh(image.ProductImage);
                            _context.ProductImages.Remove(image);
                        }
                    }
                    //Xóa hình bìa
                    if (product.ImageCover != null)
                    {
                        XoaAnh(product.ImageCover);
                    }
                    _context.Product.Remove(product);
                    _context.SaveChangesAsync();
                    return Content("1");
                }
                catch (Exception)
                {
                    return Content("0");
                }
            }
            return Content("0");
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
        public string UploadAnhBia(IFormFile Ffile)
        {
            string fileNameReturn = string.Empty;
            if (Ffile != null && Ffile.Length != 0)
            {
                fileNameReturn = $"{DateTime.Now.Ticks}{Ffile.FileName}";
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", fileNameReturn);
                using (var file = new FileStream(fullPath, FileMode.Create))
                {
                    Ffile.CopyTo(file);
                }
            }
            return fileNameReturn;
        }
        public void UploadAnhMoTa(List<IFormFile> fFiles,int id)
        {
            foreach (var myFile in fFiles)
            {
                string fileName = $"{DateTime.Now.Ticks}{myFile.FileName}";
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", fileName);
                using (var file = new FileStream(fullPath, FileMode.Create))
                {
                    myFile.CopyTo(file);
                    var productImages = new ProductImages()
                    {
                        ProductId = id,
                        ProductImage = fileName
                    };
                    _context.ProductImages.Add(productImages);
                }
            }

        }
        public void XoaAnh(string image)
        {
            if (image != null)
            {
                string fileNameBefore = image;
                string fullPathBefore = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", fileNameBefore);
                if (System.IO.File.Exists(fullPathBefore))
                {
                    System.IO.File.Delete(fullPathBefore);
                }
            }
        }
    }
}
