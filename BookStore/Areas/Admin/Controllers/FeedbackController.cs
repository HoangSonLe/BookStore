﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class FeedbackController : Controller
    {

        private readonly MyDBContext _ctx;
        public FeedbackController(MyDBContext myDBContext)
        {
            _ctx = myDBContext;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "Feedback";
            var listFB = _ctx.Feedback.AsNoTracking().ToList();
            return View(listFB);
        }

        [HttpPost]
        public int Delete(int id)
        {
            var fb = _ctx.Feedback.Where(p => p.FeedbackId == id).SingleOrDefault();
            if (fb != null)
            {
                _ctx.Feedback.Remove(fb);
                _ctx.SaveChanges();
                return 1;
            }
            return 0;
        }

        public IActionResult Detail(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var fb = _ctx.Feedback.Where(p => p.FeedbackId == id).SingleOrDefault();
            if (fb == null) return BadRequest(); 
            if (fb.EmployeeId != null)
            {
                //Feedback need to reply from staff and Someone already replied
                var emp = _ctx.Employee.SingleOrDefault(p => p.EmployeeId == fb.EmployeeId);
                ViewBag.EmployeeName = emp.FirstName + " " + emp.LastName;
            }
            return PartialView("Detail", fb);
        }

        public IActionResult Filter(int? status)
        {
            if (status == null)
            {
                return BadRequest();
            }
            var fb = _ctx.Feedback.AsQueryable();
            switch (status)
            {
                case 1:
                    {
                        //Feedback don't need to reply
                        fb = fb.Where(p => p.Reply == false);
                        break;
                    }
                case 2:
                    {
                        //Feedback need to reply
                        fb = fb.Where(p => p.Reply == true);
                        break;
                    }
                case 3:
                    {
                        //Feedback need to reply and no one has replied yet
                        fb = fb.Where(p => p.Reply == true && p.EmployeeId == null);
                        break;
                    }
                case 4:
                    {
                        //Feedback need to reply and someone has replied
                        fb = fb.Where(p => p.Reply == true && p.EmployeeId != null);
                        break;
                    }
            }
            return PartialView("Datatable", fb);
        }
    }
}