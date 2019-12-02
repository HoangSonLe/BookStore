using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Create(Orders orders)
        {
            if (ModelState.IsValid)
            {
                orders.Total = _context.OrderTemp.Sum(p => (p.Product.Price * (100 - p.Product.Discount) / 100) * p.Quantity);
                if (orders.Total < 100000)
                {
                    orders.ShipCost = 20000;
                }
                else if (orders.Total < 500000)
                {
                    orders.ShipCost = 12000;
                }
                else
                {
                    orders.ShipCost = 0;
                }
                orders.CreatedDate = DateTime.Now;
                _context.Orders.Add(orders);

                await _context.SaveChangesAsync();

                var orderdetailtmp = await _context.OrderTemp
                                            .Include(p => p.Product).ToListAsync();


                foreach (var item in orderdetailtmp)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        OrderId = orders.OrderId,
                        ProductId = item.ProductId,
                        Price = item.Product.Price,
                        Discount = item.Product.Discount,
                        Quantity = item.Quantity
                    };
                    await _context.OrderDetail.AddAsync(orderDetail);
                    _context.OrderTemp.Remove(item);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", orders.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId", orders.EmployeeId);
            return View(orders);
        }

        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetail.Where(od => od.OrderId == orders.OrderId).Include(p => p.Product).ToListAsync();

            ViewBag.orderDetails = orderDetails;

            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", orders.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "LastName", orders.EmployeeId);
            return View(orders);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,EmployeeId,Name,Address,PayMethod,ShipMethod,ShipCost,Comment,OrderStatus,ShipDate")] Orders orders)
        {
            Orders orderBefore = _context.Orders.AsNoTracking().SingleOrDefault(p => p.OrderId == orders.OrderId);
            if (orderBefore == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    orders.Total = _context.OrderDetail.Where(p => p.OrderId == orders.OrderId).Sum(p => (p.Price * (100 - p.Product.Discount) / 100 * p.Quantity));
                    orders.CreatedDate = orderBefore.CreatedDate;
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(orders.OrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", orders.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId", orders.EmployeeId);
            return View(orders);
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

        [HttpPost]
        public IActionResult AddOrderDetailTemp(int id, int quantity, string productName)
        {
            OrderTemp tmp = _context.OrderTemp.SingleOrDefault(o => o.ProductId == id);

            if (tmp != null)
            {
                tmp.Quantity += quantity;
            }
            else
            {
                tmp = new OrderTemp()
                {
                    ProductId = id,
                    ProductName = productName,
                    Quantity = quantity
                };

                _context.OrderTemp.Add(tmp);
            }
            _context.SaveChanges();
            var OrderTemps = _context.OrderTemp.OrderByDescending(p => p.OrderDetailId).ToList();
            return PartialView(OrderTemps);
        }

        [HttpDelete]
        public IActionResult DeleteOrderDetailTemp(int id)
        {
            OrderTemp tmp = _context.OrderTemp.SingleOrDefault(o => o.OrderDetailId == id);
            _context.OrderTemp.Remove(tmp);
            _context.SaveChanges();
            var OrderTemps = _context.OrderTemp.OrderByDescending(p => p.OrderDetailId).ToList();
            return PartialView("AddOrderDetailTemp", OrderTemps);
        }

        // Edit quantity Product quantity in OrderDetail
        [HttpPost]
        public OrderDetail EditQuantityOrderDetail(int orderId, int productId, int quantity)
        {
            OrderDetail orderDetail = _context.OrderDetail.SingleOrDefault(o => o.OrderId == orderId && o.ProductId == productId);
            orderDetail.Quantity = quantity;

            _context.Update(orderDetail);
            _context.SaveChangesAsync();
            return orderDetail;
        }

        //Delete orderDetail by orderID and ProductID
        [HttpDelete]
        public OrderDetail DeleteOrderDetail(int orderId, int productId)
        {
            OrderDetail tmp = _context.OrderDetail.SingleOrDefault(o => o.OrderId == orderId && o.ProductId == productId);
            _context.OrderDetail.Remove(tmp);
            _context.SaveChanges();
            return tmp;
        }

    }
}
