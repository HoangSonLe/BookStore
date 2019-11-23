using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    [Authorize(AuthenticationSchemes = "Customer")]
    [Authorize(Roles = "Customer")]
    public class ContactController : Controller
    {
        private readonly MyDBContext _ctx;
        public ContactController(MyDBContext myDBContext)
        {
            _ctx = myDBContext;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public bool Index(string name, string phone, string email, string subject, string message, int radioCheckReply)
        {
            var data = false;
            try
            {
                Feedback feedback = new Feedback()
                {
                    Name = name,
                    Email = email,
                    Phone = phone,
                    ContextSubject = subject,
                    ContextMessage = message,
                    Reply = (radioCheckReply == 0) ? false : true,
                    CreatedDate = DateTime.Now
                };
                _ctx.Add(feedback);
                _ctx.SaveChanges();
                data = true;
            }
            catch 
            {

            }
            return data;
        }
    }
}