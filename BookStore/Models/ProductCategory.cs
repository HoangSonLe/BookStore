using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            InverseParent = new HashSet<ProductCategory>();
            Product = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string UrlFriendly { get; set; }
        public int? ParentId { get; set; }
        public int? Status { get; set; }

        public virtual ProductCategory Parent { get; set; }
        public virtual ICollection<ProductCategory> InverseParent { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
