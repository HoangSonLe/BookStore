using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class OrderTemp
    {
        public int OrderDetailId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }

        public virtual Product Product { get; set; }
    }
}
