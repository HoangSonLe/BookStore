using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class ProductImages
    {
        public int ProductImagesId { get; set; }
        public int? ProductId { get; set; }
        public string ProductImage { get; set; }

        public virtual Product Product { get; set; }
    }
}
