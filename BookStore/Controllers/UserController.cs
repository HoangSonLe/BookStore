using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Models;
using BookStore.ModelViews;
using BookStore.Helpers;
using Microsoft.AspNetCore.Mvc;
using BookStore.Areas.ModelViews;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    [Authorize(AuthenticationSchemes = "Customer")]
    public class UserController : Controller
    {
        private readonly MyDBContext _ctx;
        private readonly IMapper _mapper;
        public UserController(MyDBContext myDBContext,IMapper mapper)
        {
            _ctx = myDBContext;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
   
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([Bind("UserName", "Password")] LoginViewModel loginViewModel, string ReturnUrl = null)
        {
            Customer customer = _ctx.Customer.SingleOrDefault(p => p.UserName == loginViewModel.UserName && p.Password == MyHashTool.GetMd5Hash(loginViewModel.Password));
            if (customer == null)
            {
                Employee employee = _ctx.Employee.SingleOrDefault(p => p.UserName == loginViewModel.UserName && p.Password == MyHashTool.GetMd5Hash(loginViewModel.Password));
                if (employee == null)
                {
                    ViewBag.ThongBaoLoi = "Sai thông tin đăng nhập !!!";
                    return View();
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,employee.FirstName + " " + employee.LastName),
                    new Claim(ClaimTypes.Role, employee.Role.ToString())
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Admin");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync("Admin",claimsPrincipal);
                HttpContext.Session.SetObject<Employee>("Employee", employee);
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,customer.FirstName + " " + customer.LastName),
                    new Claim(ClaimTypes.Role, "Customer")
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Customer");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync("Customer", claimsPrincipal);
                HttpContext.Session.SetObject<Customer>("Customer", customer);
            }

            ViewBag.ThongBaoThanhCong = "Thành công !!!";
            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            var b = HttpContext.Session.GetObject<Customer>("Customer");
            if (b != null)
            {
                HttpContext.Session.Remove("Customer");
                await HttpContext.SignOutAsync("Customer");
            }
            else
            {
                HttpContext.Session.Remove("Employee");
                await HttpContext.SignOutAsync("Admin");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}