using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ModelViews
{
    public class UserRegister
    {
        [Required(ErrorMessage ="Bắt buộc")]
        [MinLength(2,ErrorMessage ="Từ 2 kí tự trở lên")]
        [Remote(action: "CheckUserNameUnique", controller: "User", ErrorMessage = "Tên này đã có")]
        public string UserName { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "Bắt buộc")]
        [MinLength(2, ErrorMessage = "Từ 2 kí tự trở lên")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Bắt buộc")]
        [MinLength(2, ErrorMessage = "Từ 2 kí tự trở lên")]
        public string LastName { get; set; }
        public bool? Sex { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
