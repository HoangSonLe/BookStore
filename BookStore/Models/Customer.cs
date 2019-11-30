using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Comment = new HashSet<Comment>();
            Orders = new HashSet<Orders>();
        }

        public int CustomerId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Sex { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string RandomKey { get; set; }
        public bool? IsActive { get; set; }
        public int? Role { get; set; }
        public string AuthyId { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }

        public virtual Roles RoleNavigation { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
