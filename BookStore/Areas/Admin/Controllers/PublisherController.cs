﻿using System;
using System.IO;
using System.Linq;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin")]

    public class PublisherController : Controller
    {
        private readonly MyDBContext _ctx;
        private static string notifySuccess=null;
        public PublisherController (MyDBContext myDBContext)
        {
            _ctx = myDBContext;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "Publishers";
            var publishers = _ctx.Publishers.AsNoTracking().ToList();
            if (notifySuccess != null)
            {
                ViewBag.Message = notifySuccess;
                notifySuccess = null;//release
            }
            return View(publishers);
        }
        public int Delete(int id)
        {
            var publisher = _ctx.Publishers.SingleOrDefault(p => p.PublisherId == id);
            if (publisher != null)
            {
                try
                {
                    _ctx.Publishers.Remove(publisher);
                    _ctx.SaveChanges();
                    //delete old logo
                    string logoBefore = publisher.Logo;
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "Publisher", logoBefore);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return 0;

        }
        public IActionResult Edit(int? id)
        {
            var pub = _ctx.Publishers.SingleOrDefault(p => p.PublisherId == id);
            return PartialView("Edit",pub);
        }

        [HttpPost]
        public IActionResult Edit(IFormFile file, Publishers publisher)
        {
            var pub = _ctx.Publishers.AsNoTracking().SingleOrDefault(p => p.PublisherId == publisher.PublisherId);
            if (pub != null)//check if exist
            {
                if (file != null)
                {
                    string fileName = UploadFile(file);
                    publisher.Logo = fileName;
                    _ctx.Publishers.Update(publisher);
                    //delete old logo
                    string logoBefore = pub.Logo;
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image","Publisher", logoBefore);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                else
                {
                    // Not change logo
                    pub.PublisherName = publisher.PublisherName;
                    pub.Address = publisher.Address;
                    pub.Phone = publisher.Phone;
                    pub.Email = publisher.Email;
                    pub.Description = publisher.Description;
                    pub.UrlFrienfly = publisher.UrlFrienfly;
                    _ctx.Publishers.Update(pub);
                }
                _ctx.SaveChanges();
                notifySuccess = "Cập nhật dữ liệu thành công !!!";
                return RedirectToAction("Index");
            }
            return BadRequest();
        }
        
        public string UploadFile(IFormFile img)
        {
            string fileName = $"{DateTime.Now.Ticks}{img.FileName}";
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" ,"Image", "Publisher", fileName);
            using (var f = new FileStream(fullPath, FileMode.Create))
            {
                img.CopyTo(f);
            }
            return fileName;
        }

        public IActionResult Create()
        {
            return PartialView("Create");
        }

        [HttpPost]
        public IActionResult Create(IFormFile file, Publishers publisher)
        {
            publisher.Logo = UploadFile(file);
            _ctx.Publishers.Add(publisher);
            _ctx.SaveChanges();
            notifySuccess = "Thêm dữ liệu thành công !!!";
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int? id)
        {
            if (id == null) return BadRequest();
            var pub = _ctx.Publishers.SingleOrDefault(p => p.PublisherId == id);
            return PartialView("Detail", pub);
        }

    }
}