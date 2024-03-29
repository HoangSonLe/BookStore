﻿using BookStore.Helpers;
using BookStore.Models;
using BookStore.ModelViews;
using BraintreeHttp;
using Microsoft.AspNetCore.Mvc;
using PayPal.Core;
using PayPal.v1.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class PaypalController : Controller
    {
        private double VND_USD = 23000;
        private readonly MyDBContext _context;
        private static int IdLasted = 0;

        public PaypalController(MyDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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

        public async Task<IActionResult> Checkout(Orders order)
        {
            //SandboxEnvironment(clientId, clientSerect)
            var environment = new SandboxEnvironment("ATEyDHMWKozlcGDf5yNduG92WTeajJf9gqXc34Dd0AU7LbWgFvH3qY_8ImvFfZls5uZMzaoeZAdZBCrm", "EH1J-u4MfbsyBENy8zBoVHHOnU9DMsDRjGDaXwfZltNEVc3rv_t26ANZ_L2Z4eQlS12oRnUj_Zr8dizO");
            var client = new PayPalHttpClient(environment);

            //Đọc thông tin đơn hàng từ Session
            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };

            var AmountUSD = Cart.Sum(p => CurrencyConversion(p.Price, p.QuantityProduct));
            int? AmountVND = 0;

            foreach (var item in Cart)
            {
                AmountVND += item.Price;
                itemList.Items.Add(new Item()
                {
                    Name = item.ProductName,
                    Currency = "USD",
                    Price = CurrencyConversion(item.Price).ToString(),
                    Quantity = item.QuantityProduct.ToString(),
                    Tax = "0"
                });
            }

            order.CreatedDate = DateTime.Now;

            // tính phí ship
            order.Total = AmountVND;
            var shipCostUSD = 0.00;
            if (AmountVND < 100000)
            {
                shipCostUSD = 0.87;
                order.ShipCost = 20000;
            }
            else if (AmountVND < 500000)
            {
                shipCostUSD = 0.52;
                order.ShipCost = 12000;
            }
            else
            {
                order.ShipCost = 0;
            }

            // kiểm tra người dùng có đăng nhập để lưu ID người dùng
            var info = HttpContext.Session.GetObject<Customer>("Customer");
            if (info != null)
            {
                order.CustomerId = info.CustomerId;
            }

            _context.Orders.Add(order);
            _context.SaveChanges();
            IdLasted = order.OrderId;
            foreach (var item in Cart)
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

            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = (AmountUSD+shipCostUSD).ToString(),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = shipCostUSD.ToString(),
                                Subtotal = AmountUSD.ToString(),
                            }
                        },
                        ItemList = itemList,
                        Description = "Don hang " + IdLasted,
                        InvoiceNumber = DateTime.Now.Ticks.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/Paypal/Fail",
                    ReturnUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/Paypal/Execute"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            try
            {
                HttpResponse response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                var links = result.Links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment
                        paypalRedirectUrl = lnk.Href;
                    }
                }

                return Redirect(paypalRedirectUrl);
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                return RedirectToAction("Fail");
            }
        }

        private double CurrencyConversion(int? price, int quantityProduct = 1)
        {
            var currency = ((double)price / VND_USD).ToString("0.00");
            return double.Parse(currency) * quantityProduct;
        }

        public IActionResult Success()
        {
            //Tạo đơn hàng trong CSDL với trạng thái : Đã thanh toán, phương thức: Paypal
            Orders order = _context.Orders.FirstOrDefault(o => o.OrderId == IdLasted);
            order.OrderStatus = (int?)1;
            _context.SaveChanges();
            return RedirectToAction("Invoice", "Checkout", order);
        }

        public IActionResult Fail()
        {
            //Tạo đơn hàng trong CSDL với trạng thái : Chưa thanh toán, phương thức:
            Orders order = _context.Orders.FirstOrDefault(o => o.OrderId == IdLasted);
            order.OrderStatus = (int?)0;
            _context.SaveChanges();
            return RedirectToAction("PaymentFailed", "Checkout");
        }
        public async Task<IActionResult> Execute(string paymentId, string PayerId)
        {
            //SandboxEnvironment(clientId, clientSerect)
            string clientId = "ATEyDHMWKozlcGDf5yNduG92WTeajJf9gqXc34Dd0AU7LbWgFvH3qY_8ImvFfZls5uZMzaoeZAdZBCrm";
            string clientSecret = "EH1J-u4MfbsyBENy8zBoVHHOnU9DMsDRjGDaXwfZltNEVc3rv_t26ANZ_L2Z4eQlS12oRnUj_Zr8dizO";
            var environment = new SandboxEnvironment(clientId, clientSecret);
            var client = new PayPalHttpClient(environment);


            PaymentExecuteRequest request = new PaymentExecuteRequest(paymentId);

            request.RequestBody(new PaymentExecution()
            {
                PayerId = PayerId
            });

            try
            {
                HttpResponse response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();
                return RedirectToAction("Success");
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();
                return RedirectToAction("Fail");
            }

        }
    }
}