using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class ProductLike
    {
        public int ProductLikeId { get; set; }
        public int? ProductId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
