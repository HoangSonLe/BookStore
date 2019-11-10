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

namespace BookStore.Controllers
{
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
            return View("../Home/Index");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public string Login(String username, String password)
        {
            var passwordMD5 = GetMD5(password);
            var IsSuccessLogin = "false";
            var customer = _ctx.Customer.Where(p => p.UserName == username && p.Password == passwordMD5).SingleOrDefault();
            if (customer != null)
            {
                LoginInfo loginInfo = new LoginInfo()
                {
                    UserID = customer.CustomerId,
                    Name = customer.FirstName + " " + customer.LastName,
                    //Role = customer.Roles.RoleId,
                };
                HttpContext.Session.SetObject<LoginInfo>("Info", loginInfo);
                IsSuccessLogin = "true";
            }
            else
            {
                //return Redirect("/");

            }
            //return Redirect("/");
            return IsSuccessLogin;
        }
        public string GetMD5(string str)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sbHash = new StringBuilder();

            foreach (byte b in bHash)
            {

                sbHash.Append(String.Format("{0:x2}", b));

            }

            return sbHash.ToString();

        }
    }
}