using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;
using BookStore.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class OrderController : Controller
    {
        private readonly MyDBContext _context;

        public OrderController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.Orders.Include(o => o.Customer).Include(o => o.Employee);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orders == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetail.Where(od => od.OrderId == orders.OrderId).Include(p => p.Product).ToListAsync();

            ViewBag.orderDetails = orderDetails;

            return PartialView(orders);
        }

        // GET: Admin/Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId");

            var info = HttpContext.Session.GetObject<Employee>("Employee");
            ViewBag.Employee = info;
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductName");
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Address,Phone,Email,PayMethod,ShipMethod,ShipCost,Comment,OrderStatus,ShipDate")] Orders order,
                                        [Bind("ArrAddItemId")] string ArrAddItemId,
                                        [Bind("ArrAddItemQuantity")] string ArrAddItemQuantity)
        {
            var emp = HttpContext.Session.GetObject<Employee>("Employee");
            if (emp == null)
            {
                return RedirectToAction("Index", "Login");
            }
            //mảng id item thêm
            string[] arrAddItemId = new string[] { };
            if (ArrAddItemId.Length > 0)
            {
                ArrAddItemId = ArrAddItemId.Trim();
                arrAddItemId = ArrAddItemId.Split(',');
            }
            //mảng quantity item sửa
            string[] arrAddItemQuantity = new string[] { };
            if (ArrAddItemQuantity.Length > 0)
            {
                ArrAddItemQuantity = ArrAddItemQuantity.Trim();
                arrAddItemQuantity = ArrAddItemQuantity.Split(',');
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(order);
                    _context.SaveChanges();
                    int? total = 0;
                    if (ArrAddItemId.Count() > 0)
                    {
                        for (var i = 0; i < arrAddItemId.Count(); ++i)
                        {
                            try
                            {
                                Product p = _context.Product.AsNoTracking().SingleOrDefault(o => o.ProductId == int.Parse(arrAddItemId[i].Trim()));
                                if (p != null)
                                {
                                    OrderDetail detail = new OrderDetail();
                                    detail.OrderId = order.OrderId;
                                    detail.ProductId = p.ProductId;
                                    detail.Quantity = int.Parse(arrAddItemQuantity[i].Trim());
                                    detail.Price = (p.Discount == 0) ? p.Price : p.PromotionPrice;
                                    _context.Update(detail);
                                    total += detail.Price * detail.Quantity;
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }

                    }
                    order.EmployeeId = emp.EmployeeId;
                    order.Total = total + (int)order.ShipCost;
                    order.CreatedDate = DateTime.Now;
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            var orderDetails = await _context.OrderDetail.Where(od => od.OrderId == order.OrderId).Include(p => p.Product).ToListAsync();
            ViewBag.orderDetails = orderDetails;

            ViewData["EmployeeName"] = emp.FirstName + " " + emp.LastName;
            return View("Edit",order);
        }


        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetail.Where(od => od.OrderId == order.OrderId).Include(p => p.Product).ToListAsync();

            ViewBag.orderDetails = orderDetails;
            if (order.EmployeeId != null)
            {
                var emp = _context.Employee.SingleOrDefault(p => p.EmployeeId == order.EmployeeId);
                if (emp != null)
                {
                    ViewData["EmployeeName"] = emp.FirstName +" "+ emp.LastName;
                }
                else ViewData["EmployeeName"] = "";
            }
            else ViewData["EmployeeName"] = "";

            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,Name,Address,Phone,Email,PayMethod,ShipMethod,ShipCost,Comment,OrderStatus,ShipDate")] Orders order,
                                                [Bind("ArrDeleteItem")] string ArrDeleteItem,
                                                [Bind("ArrEditItemId")] string ArrEditItemId,
                                                [Bind("ArrEditItemQuantity")] string ArrEditItemQuantity)
        {
            var emp = HttpContext.Session.GetObject<Employee>("Employee");
            if (emp == null)
            {
                 return RedirectToAction("Index", "Login");
            }
            //mảng id item xóa
            string[] arrDeleteItem = new string[] { };
            if (ArrDeleteItem.Length > 0)
            {
                ArrDeleteItem = ArrDeleteItem.Trim();
                arrDeleteItem = ArrDeleteItem.Split(',');
            }
            //mảng id item sửa
            string[] arrEditItemId = new string[] { };
            if (ArrDeleteItem.Length > 0)
            {
                ArrEditItemId = ArrEditItemId.Trim();
                arrEditItemId = ArrEditItemId.Split(',');
            }
            //mảng id hình ảnh xóa
            string[] arrEditItemQuantity = new string[] { };
            if (ArrEditItemQuantity.Length > 0)
            {
                ArrEditItemQuantity = ArrEditItemQuantity.Trim();
                arrEditItemQuantity = ArrEditItemQuantity.Split(',');
            }
            Orders orderBefore = _context.Orders.AsNoTracking().SingleOrDefault(p => p.OrderId == order.OrderId);
            if (orderBefore == null)
            {
                return NotFound();
            }
            order.Total = orderBefore.Total-(int)orderBefore.ShipCost;
            if (ModelState.IsValid)
            {
                try
                {
                    if (arrDeleteItem.Count() > 0)
                    {
                        foreach(var o in arrDeleteItem)
                        {
                            try
                            {
                                OrderDetail orderDetail = _context.OrderDetail.AsNoTracking().SingleOrDefault(p => p.ProductId == int.Parse(o.Trim()));
                                order.Total -= orderDetail.Price * orderDetail.Quantity;
                                _context.Remove(orderDetail);
                            }
                            catch (Exception)
                            {

                            }
                        }

                    }
                    if (arrEditItemId.Count() > 0)
                    {
                        for (var i=0;i<arrEditItemId.Count();++i)
                        {
                            try
                            {
                                OrderDetail orderDetail = _context.OrderDetail.SingleOrDefault(p => p.ProductId == int.Parse(arrEditItemId[i].Trim()));
                                order.Total= order.Total - (orderDetail.Price * orderDetail.Quantity);
                                orderDetail.Quantity = int.Parse(arrEditItemQuantity[i].Trim());
                                _context.Update(orderDetail);
                                order.Total =order.Total + (orderDetail.Price * orderDetail.Quantity);
                            }
                            catch (Exception)
                            {

                            }
                        }

                    }
                    order.EmployeeId =emp.EmployeeId;
                    order.Total= order.Total+ (int)order.ShipCost;
                    order.CreatedDate = orderBefore.CreatedDate;
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    { 
                        throw;
                    }
                }
            }
            var orderDetails = await _context.OrderDetail.Where(od => od.OrderId == order.OrderId).Include(p => p.Product).ToListAsync();
            ViewBag.orderDetails = orderDetails;

            ViewData["EmployeeName"] = emp.FirstName + " " + emp.LastName;
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orders = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        public IActionResult AddProductToList(int productId, int quantity)
        {
            var product = _context.Product.AsNoTracking().SingleOrDefault(p => p.ProductId == productId);
            ViewBag.Quantity = quantity;
            return PartialView("ProductItem", product);
        }

    }
}
