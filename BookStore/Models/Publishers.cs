using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class Publishers
    {
        public Publishers()
        {
            Product = new HashSet<Product>();
        }

        public int PublisherId { get; set; }
        public string PublisherName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
