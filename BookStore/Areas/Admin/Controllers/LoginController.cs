using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookStore.Areas.ModelViews;
using BookStore.Helpers;
using BookStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes ="Admin")]
    public class LoginController : Controller
    {
        private readonly MyDBContext _ctx;
        public LoginController(MyDBContext myDBContext)
        {
            _ctx = myDBContext;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost,AllowAnonymous]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel ,string ReturnUrl = null)
        {
            Employee employee = _ctx.Employee.SingleOrDefault(p => p.UserName == loginViewModel.UserName && p.Password == MyHashTool.GetMd5Hash(loginViewModel.Password));
            if(employee == null)
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
            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            HttpContext.Session.SetObject<Employee>("Employee", employee);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("Employee");
            await HttpContext.SignOutAsync("Admin");
            return RedirectToAction("Index");
        }
    }
}