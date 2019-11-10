using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int? ProductId { get; set; }
        public int? CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public string Context { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Status { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Product Product { get; set; }
    }
}
