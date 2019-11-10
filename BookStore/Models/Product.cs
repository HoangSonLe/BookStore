using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class Product
    {
        public Product()
        {
            Comment = new HashSet<Comment>();
            OrderDetail = new HashSet<OrderDetail>();
            ProductImages = new HashSet<ProductImages>();
            ProductLike = new HashSet<ProductLike>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public string UrlFriendly { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }
        public int? PromotionPrice { get; set; }
        public bool? IncludeVat { get; set; }
        public int? Quantity { get; set; }
        public int? CategoryId { get; set; }
        public int? PublisherId { get; set; }
        public int? Discount { get; set; }
        public int? ViewCounts { get; set; }
        public bool? Status { get; set; }

        public virtual ProductCategory Category { get; set; }
        public virtual Publishers Publisher { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
        public virtual ICollection<ProductImages> ProductImages { get; set; }
        public virtual ICollection<ProductLike> ProductLike { get; set; }
    }
}
