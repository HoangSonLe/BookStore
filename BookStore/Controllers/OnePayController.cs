using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Helpers;
using BookStore.Models;
using BookStore.ModelViews;
using EMarket.Services.OnePay;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class OnePayController : Controller
    {
        private double VND_USD = 23000;
        private readonly MyDBContext _context;
        private IHttpContextAccessor _accessor;
        private static int IdLasted = 0;

        public OnePayController(MyDBContext context)
        {
            _context = context;
        }
        public List<CartItem> Cart
        {
            get
            {
                var data = HttpContext.Session.GetObject<List<CartItem>>("Cart");
                if (data == null)
                {
                    data = new List<CartItem>();
                }

                return data;
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OnePayPayment(Orders order)
        {
            var current_invoice = CreateInvoice(order);


            //Send request to OnePay
            string returnURL = Url.Action("OnePayResult", "OnePay", null, Request.Scheme);
            VPCRequest conn = new VPCRequest();
            conn.SetSecureSecret(VPCRequest.SECURE_SECRET);
            conn.AddDigitalOrderField("Title", "onepay paygate");
            conn.AddDigitalOrderField("vpc_Locale", "vn");//Chon ngon ngu hien thi tren cong thanh toan (vn/en)
            conn.AddDigitalOrderField("vpc_Version", "2");
            conn.AddDigitalOrderField("vpc_Command", "pay");
            conn.AddDigitalOrderField("vpc_Merchant", VPCRequest.MERCHANT_ID);
            conn.AddDigitalOrderField("vpc_AccessCode", VPCRequest.ACCESS_CODE);
            conn.AddDigitalOrderField("vpc_MerchTxnRef", "HoaDon #:" + current_invoice.OrderId);
            conn.AddDigitalOrderField("vpc_OrderInfo", "HoaDon #:" + current_invoice.OrderId);
            conn.AddDigitalOrderField("vpc_Amount", current_invoice.Total + "00");
            conn.AddDigitalOrderField("vpc_Currency", "VND");
            conn.AddDigitalOrderField("vpc_ReturnURL", returnURL);

            // Thong tin them ve khach hang. De trong neu khong co thong tin
            conn.AddDigitalOrderField("vpc_Customer_Phone", order.Phone);
            conn.AddDigitalOrderField("vpc_Customer_Email", order.Email);

            // Chuyen huong trinh duyet sang cong thanh toan
            string url = conn.Create3PartyQueryString();
            return Redirect(url);
        }

        private Orders CreateInvoice(Orders order)
        {

            var amount = Cart.Sum(c => c.QuantityProduct * c.Price);
            order.CreatedDate = DateTime.Now;
            order.Total = amount;
            _context.Orders.Add(order);
            _context.SaveChanges();
            IdLasted = order.OrderId;

            foreach(var item in Cart)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderId = IdLasted,
                    ProductId = item.ProductId,
                    Price = item.Price,
                    Quantity = item.QuantityProduct
                };
                _context.OrderDetail.Add(orderDetail);
                _context.SaveChanges();
            }

            return order;
        }

        public IActionResult OnePayResult(string vpc_TxnResponseCode)
        {
            //"http://onepay.vn"
            VPCRequest conn = new VPCRequest();
            conn.SetSecureSecret(VPCRequest.SECURE_SECRET);
            // Xu ly tham so tra ve va kiem tra chuoi du lieu ma hoa
            var hashvalidateResult = conn.Process3PartyResponse(HttpContext.Request.Query);
            Orders order = _context.Orders.FirstOrDefault(o => o.OrderId == IdLasted);
            if (hashvalidateResult == "CORRECTED" && vpc_TxnResponseCode.Trim() == "0")
            {
                order.OrderStatus = (int?)1;
                _context.SaveChanges();
                return RedirectToAction("Invoice", "Checkout", order);
            }
            else if (hashvalidateResult == "INVALIDATED" && vpc_TxnResponseCode.Trim() == "0")
            {
            }
            order.OrderStatus = (int?)0;
            _context.SaveChanges();
            return RedirectToAction("PaymentFailed", "Checkout");
        }
    }
}