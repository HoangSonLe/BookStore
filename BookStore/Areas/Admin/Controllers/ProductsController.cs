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
            ViewData["CategoryId"] = new SelectList(_context.ProductCategory, "CategoryId", "CategoryId");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,Unit,UrlFriendly,Description,Price,PromotionPrice,IncludeVat,Quantity,CategoryId,PublisherId,Discount,ViewCounts,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.ProductCategory, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName", product.PublisherId);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(m => m.ProductImages).AsNoTracking().SingleOrDefaultAsync(p=> p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.ProductImages = _context.ProductImages.Where(p => p.ProductId == id).ToList();
            //ViewData["CategoryId"] = new SelectList(_context.ProductCategory, "CategoryId", "Name", product.CategoryId);
            ViewData["CategoryId"] = new SelectList(_context.ProductCategory, "CategoryId", "Name", product.CategoryId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName", product.PublisherId);
            //ViewData["ProductImages"] = new SelectList(_context.ProductImages, "ProductId", "ProductImage", product.ProductId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Unit,UrlFriendly,Description,Price,PromotionPrice,IncludeVat,Quantity,CategoryId,PublisherId,Discount,ViewCounts,Status")] Product product, List<IFormFile> fFiles)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //if (fFiles == null || fFiles.Count == 0)
                    //    return Content("files not selected");
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

                    _context.Update(product);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.ProductCategory, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName", product.PublisherId);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Publisher)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
