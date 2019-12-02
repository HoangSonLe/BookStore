using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ModelViews
{
    public class UserProfileView
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityCardNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool? Sex { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string ManagerName { get; set; }
        public string Image { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public int Role { get; set; }
    }
}
