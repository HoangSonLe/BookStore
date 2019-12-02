using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.ModelViews;
using BookStore.Helpers;
using Microsoft.AspNetCore.Mvc;
using BookStore.Areas.ModelViews;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using BookStore.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Newtonsoft.Json;
using BookStore.Common;
using AutoMapper;

namespace BookStore.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    [Authorize(AuthenticationSchemes = "Customer")]
    public class UserController : Controller
    {
        private readonly MyDBContext _ctx;
        private readonly IAuthy _authy;
        private readonly ISmsService _smsService;
        private readonly IMapper _mapper;

        public UserController(MyDBContext myDBContext, IAuthy auth, ISmsService smsService,IMapper mapper)
        {
            _ctx = myDBContext;
            _authy = auth;
            _smsService = smsService;
            _mapper = mapper;
        }
        [AllowAnonymous]
        public IActionResult CheckUserNameUnique(string UserName)
        {
            Customer customerSimilar = _ctx.Customer.AsNoTracking().FirstOrDefault(p => p.UserName == UserName);
            if (customerSimilar != null)
            {
                return Json(false);
            }
            return Json(true);
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
            Customer customer = _ctx.Customer.AsNoTracking().SingleOrDefault(p => p.UserName == loginViewModel.UserName && p.Password == MyHashTool.GetMd5Hash(loginViewModel.Password));
            if(customer!=null)
            {
                if (customer.PhoneNumberConfirmed == false)
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
                    TempData["ThongBao"] = "Đăng nhập thành công";
                }
                else if(customer.PhoneNumberConfirmed==true && customer.AuthyId != null)
                {
                    // Gửi sms mã xác nhận
                    var sendSMSResponse = await _authy.SendSmsAsync(customer.AuthyId).ConfigureAwait(false);
                    if (sendSMSResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var smsVerificationSucceedObject = JsonConvert.DeserializeObject<AccessCodeVerifyResult>(await sendSMSResponse.Content.ReadAsStringAsync());
                        if (smsVerificationSucceedObject.Success)
                        {
                            ViewBag.CustomerId = customer.CustomerId;
                            ViewBag.ResultSMS = "Gửi mã thành công!";
                            //Send SMS success
                            return View("VerifyUser");

                        }
                        else
                        {
                            ViewBag.CustomerId = customer.CustomerId;
                            ViewBag.ResultSMS = "Gửi mã thất bại!";
                            //Fail
                            return View("VerifyUser");
                        }
                    }
                    else
                    {
                        ViewBag.ResultSMS = "Gửi mã thất bại!";
                        return View("Login");
                    }
                }
            }
            else
            {
                ViewBag.ResultLogin = "Không có tên đăng nhập này!";
                return View();
            }

            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("Customer");
            await HttpContext.SignOutAsync("Customer");
            HttpContext.Session.Remove("Admin");
            await HttpContext.SignOutAsync("Admin");
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            var tmp = HttpContext.Session.GetObject<Customer>("Customer");
            if (tmp != null)
            {
                HttpContext.Session.Remove("Customer");
                await HttpContext.SignOutAsync("Customer");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([Bind("UserName", "Password", "FirstName", "LastName", "Sex", "Address", "Email", "PhoneNumber")] UserRegister userRegister,[Bind("TwoFactorCheck")] bool TwoFactorCheck, [Bind("fFile")] IFormFile fFile)
        {
            Customer customerSimilar = _ctx.Customer.AsNoTracking().FirstOrDefault(p => p.UserName == userRegister.UserName);
            if (customerSimilar == null)
            {
                Customer customer = _mapper.Map<UserRegister, Customer>(userRegister);
                if (ModelState.IsValid)
                {
                    //thiếu check mail, phone,...
                    customer.Password = MyHashTool.GetMd5Hash(customer.Password);
                    //thêm ảnh đại diện
                    string fileName = UploadAnh(fFile);
                    if (fileName != null)
                    {
                        customer.Image = fileName;
                    }
                    else
                    {
                        customer.Image = "";
                    }
                    customer.IsActive = true;
                    Roles role = _ctx.Roles.AsNoTracking().SingleOrDefault(p => p.RoleName == "Customer");
                    customer.Role = role.RoleId;
                    customer.PhoneNumberConfirmed = false;
                    UserModel userModel = new UserModel
                    {
                        Email = customer.Email,
                        CountryCode = "+84",
                        PhoneNumber = (customer.PhoneNumber.Length > 9) ? customer.PhoneNumber.Substring(1) : customer.PhoneNumber
                    };
                    //Lấy authy id
                    var authyId = await _authy.RegisterUserAsync(userModel).ConfigureAwait(false);
                    _ctx.Add(customer);
                    _ctx.SaveChanges();

                    if (string.IsNullOrEmpty(authyId))
                    {
                        //return Json(new { success = false });
                        ViewBag.RegisterResult = "Đăng ký thành công";
                        ViewBag.RegisterSMS = "Xác thực số điện thoại thất bại";
                        return View("Login");
                    }
                    else
                    {
                        //update authyId in database

                        customer.AuthyId = authyId;
                        _ctx.Update(customer);
                        await _ctx.SaveChangesAsync();
                        if (TwoFactorCheck == true)
                        {
                            ViewBag.CustomerId = customer.CustomerId;
                            // Gửi sms mã xác nhận
                            var sendSMSResponse = await _authy.SendSmsAsync(customer.AuthyId).ConfigureAwait(false);

                            if (sendSMSResponse.StatusCode == HttpStatusCode.OK)
                            {
                                var smsVerificationSucceedObject = JsonConvert.DeserializeObject<AccessCodeVerifyResult>(await sendSMSResponse.Content.ReadAsStringAsync());
                                if (smsVerificationSucceedObject.Success)
                                {
                                    ViewBag.CustomerId = customer.CustomerId;
                                    ViewBag.ResultSMS = "Gửi mã thành công!";
                                    //Send SMS success
                                    return View("VerifyUser");

                                }
                                else
                                {
                                    ViewBag.ResultSMS = "Gửi mã thất bại!";
                                    ViewBag.CustomerId = customer.CustomerId;
                                    //Fail
                                    return View("VerifyUser");
                                }
                            }
                            else
                            {
                                ViewBag.ResultSMS = "Gửi mã thất bại!";
                                return View("Login");
                            }
                        }
                        else
                        {
                            ViewBag.RegisterResult = "Đăng ký thành công";
                            return View("Login");
                        }                        
                    }
                }
            }
            ViewBag.RegisterResult = "Trùng tên đăng nhập hoặc mật khẩu";
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyFactor([Bind("token")] string token,[Bind("CustomerId")] int CustomerId)
        {
            try
            {
                Customer customer = _ctx.Customer.AsNoTracking().SingleOrDefault(kh => kh.CustomerId == CustomerId);
                if (customer != null && !string.IsNullOrEmpty(customer.AuthyId))
                {
                    var validationResult = await _authy.VerifyTokenAsync(customer.AuthyId, token).ConfigureAwait(false);

                    if (validationResult.Succeeded)
                    {
                        customer.PhoneNumberConfirmed = true;
                        _ctx.Update(customer);
                        _ctx.SaveChanges();
                        var claims = new List<Claim>
                                            {
                                                new Claim(ClaimTypes.Name,customer.FirstName + " " + customer.LastName),
                                                new Claim(ClaimTypes.Role, "Customer")
                                            };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Customer");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync("Customer", claimsPrincipal);
                        HttpContext.Session.SetObject<Customer>("Customer", customer);
                        TempData["ThongBao"] = "Đăng ký thành công. Bảo mật 2 lớp thành công!!!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.ResultVerify =
                        $"Không thể xác minh +84{customer.PhoneNumber}. Vui lòng kiểm tra SĐT hoặc mã đã nhập có đúng không.";
                    }
                }
                
            }
            catch (Exception e)
            {
                ViewBag.Result = e.Message;
            }
            ViewBag.CustomerId = CustomerId;
            //Fail
            return View("VerifyUser");
        }
        public string UploadAnh(IFormFile fFile)
        {
            if (fFile != null && fFile.Length != 0)
            {
                string fileName = $"{DateTime.Now.Ticks}{fFile.FileName}";
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "Customer", fileName);
                using (var file = new FileStream(fullPath, FileMode.Create))
                {
                    fFile.CopyTo(file);
                    return fileName;
                }
            }
            return null;
        }

        public IActionResult Profile()
        {   
            //Getting session
            var cus = HttpContext.Session.GetObject<Customer>("Customer");
            var emp = HttpContext.Session.GetObject<Employee>("Employee");
            if (cus != null)
            {
                var user = new UserProfileView
                {
                    UserID = cus.CustomerId,
                    UserName = cus.UserName,
                    Password = cus.Password,
                    FirstName = cus.FirstName,
                    LastName = cus.LastName,
                    Address = cus.Address,
                    Email = cus.Email,
                    Sex = cus.Sex,
                    Phone = cus.PhoneNumber,
                    PhoneNumberConfirmed = cus.PhoneNumberConfirmed,
                    Image = cus.Image,
                    Role = 1
                };
                ViewBag.Orders = _ctx.Orders.AsNoTracking().Where(p => p.CustomerId == cus.CustomerId);
                return View(user);
            }
            else if (emp != null)
            {
                var manager = _ctx.Employee.SingleOrDefault(p => p.EmployeeId == emp.ManagerId);
                string name = "";
                if (manager!=null) name = manager.FirstName + " " + manager.LastName;
                var user = new UserProfileView
                {
                    UserID = emp.EmployeeId,
                    UserName = emp.UserName,
                    Password = emp.Password,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Address = emp.Address,
                    IdentityCardNumber = emp.IdentityCardNumber,
                    BirthDate = emp.BirthDate,
                    ManagerName = name,
                    Email = emp.Email,
                    Sex = emp.Sex,
                    Phone = emp.Phone,
                    Image = emp.Image,
                    Role=2
                };
                ViewBag.Orders = _ctx.Orders.AsNoTracking().Where(p => p.EmployeeId == emp.EmployeeId);
                return View(user);

            }
            return BadRequest();
            
        }

        public IActionResult GetOrderDetail(int OrderID)
        {
            var detail = _ctx.OrderDetail.AsNoTracking().Where(p => p.OrderId == OrderID).Include(x=>x.Product);
            return PartialView("ModalOrderDetail", detail);
        }

        [HttpPost]
        public int ChangePassword(string OldPass, string NewPass)
        {
            string hashOldPass = MyHashTool.GetMd5Hash(OldPass);
            var cus = HttpContext.Session.GetObject<Customer>("Customer");
            var emp = HttpContext.Session.GetObject<Employee>("Employee");
            if (cus != null)
            {
                if (cus.Password == hashOldPass)
                {
                    //They are same
                    var c = _ctx.Customer.SingleOrDefault(p => p.CustomerId == cus.CustomerId);
                    c.Password = MyHashTool.GetMd5Hash(NewPass);
                    _ctx.Customer.Update(c);
                    _ctx.SaveChanges();
                    return 1;
                }
                
            }
            else
            {
                if (emp.Password == hashOldPass)
                {
                    //They are same
                    var e = _ctx.Employee.SingleOrDefault(p => p.EmployeeId == emp.EmployeeId);
                    e.Password = MyHashTool.GetMd5Hash(NewPass);
                    _ctx.Employee.Update(e);
                    _ctx.SaveChanges();
                    return 1;
                }
               
            }
            return 0;
        }
        [HttpPost]
        public IActionResult UploadImage([FromForm]IFormFile file)
        {
            var cus = HttpContext.Session.GetObject<Customer>("Customer");
            var emp = HttpContext.Session.GetObject<Employee>("Employee");
            string folder = "";
            if (cus != null)
            {
                folder = cus.CustomerId + "-" + cus.Role;
            }
            else
            {
                folder = emp.EmployeeId + "-" + emp.Role;
            }
            if (file != null)
            {
                var name = UploadHinh(file, folder);
                return Ok(new { name = name });
            }
            return Ok();
        }
        private void MoveImage(string NameImage)
        {
            
            var cus = HttpContext.Session.GetObject<Customer>("Customer");
            var emp = HttpContext.Session.GetObject<Employee>("Employee");
            string tmpFolder = "";
            string destinationFolder = "";
            string oldImagePath = "";
            if (cus != null)
            {
                tmpFolder = cus.CustomerId + "-" + cus.Role;
                destinationFolder = "Customer";
                if (!string.IsNullOrEmpty(cus.Image))
                {
                    oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", destinationFolder, cus.Image);
                }
            }
            else
            {
                tmpFolder = emp.EmployeeId + "-" + emp.Role;
                destinationFolder = "Employee";
                if (!string.IsNullOrEmpty(emp.Image))
                {
                    oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", destinationFolder, emp.Image);

                }
            }

            /*----Start Move file from one folder to another folder*/
            var sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", tmpFolder, NameImage);
            var destinationPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", destinationFolder, NameImage);
            if (System.IO.File.Exists(sourcePath))
            {
                System.IO.File.Move(sourcePath, destinationPath);
            }
            /*----End Move file from one folder to another folder*/

            /*----Start Delete file from folder*/
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", tmpFolder);
            System.IO.DirectoryInfo di = new DirectoryInfo(dir);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            /*----End Delete file from folder*/

            /*----Delete Old Image-----------*/
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

        }

        [HttpPost]
        public IActionResult UpdateInfo(UserProfileView profile, string NameImage)
        {
            //Lấy session để biết người này là employee hay customer
            var cus = HttpContext.Session.GetObject<Customer>("Customer");
            var emp = HttpContext.Session.GetObject<Employee>("Employee");
            
            if (cus != null)
            {
                if (!string.IsNullOrEmpty(NameImage))
                {
                    MoveImage(NameImage);//Move Image and delete OldImage
                }
                var customer = _ctx.Customer.SingleOrDefault(p => p.CustomerId == cus.CustomerId);
                customer.FirstName = profile.FirstName;
                customer.LastName = profile.LastName;
                customer.Address = profile.Address;
                customer.Email = profile.Email;
                customer.Sex = profile.Sex;
                customer.PhoneNumber = profile.Phone;
                customer.PhoneNumberConfirmed = profile.PhoneNumberConfirmed;
                if (!string.IsNullOrEmpty(NameImage)) customer.Image = NameImage;

                //Update SQL
                _ctx.Customer.Update(customer);
                _ctx.SaveChanges();
                var claims = new List<Claim>
                                            {
                                                new Claim(ClaimTypes.Name,customer.FirstName + " " + customer.LastName),
                                                new Claim(ClaimTypes.Role, "Customer")
                                            };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Customer");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                HttpContext.SignInAsync("Customer", claimsPrincipal);

                //Update Session
                HttpContext.Session.SetObject<Customer>("Customer", customer);

            }
            else
            {
                if (!string.IsNullOrEmpty(NameImage))
                {
                    MoveImage(NameImage);//Move Image and delete OldImage
                }
                var employee = _ctx.Employee.SingleOrDefault(p => p.EmployeeId == emp.EmployeeId);
                employee.FirstName = profile.FirstName;
                employee.LastName = profile.LastName;
                employee.Address = profile.Address;
                employee.IdentityCardNumber = profile.IdentityCardNumber;
                employee.BirthDate = profile.BirthDate;
                employee.Email = profile.Email;
                employee.Sex = profile.Sex;
                employee.Phone = profile.Phone;
                if (!string.IsNullOrEmpty(NameImage)) employee.Image = NameImage;
                //Update SQL
                _ctx.Employee.Update(employee);
                _ctx.SaveChanges();
                //Update Session
                HttpContext.Session.SetObject<Employee>("Employee", employee);

                //
                var claims = new List<Claim>
                {
                   new Claim(ClaimTypes.Name,employee.FirstName + " " + employee.LastName),
                   new Claim(ClaimTypes.Role, employee.Role.ToString())
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Admin");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                HttpContext.SignInAsync("Admin", claimsPrincipal);
            }
            
            return this.Ok();
        }

        public string UploadHinh(IFormFile fHinh, string folder)
        {
            string fileNameReturn = string.Empty;
            if (fHinh != null)
            {
                fileNameReturn = $"_{DateTime.Now.Ticks}{fHinh.FileName}";
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", folder);
                // Kiểm tra thư mục có tồn tại ko
                // nếu ko tồn tại thì tạo mới
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                var fileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", folder,fileNameReturn);

                using (var file = new FileStream(fileName, FileMode.Create))
                {
                    fHinh.CopyTo(file);
                }
            }
            return fileNameReturn;
        }
    }
}