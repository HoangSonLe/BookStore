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
            ProductLike = new HashSet<ProductLike>();
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
        public string Phone { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<ProductLike> ProductLike { get; set; }
    }
}
