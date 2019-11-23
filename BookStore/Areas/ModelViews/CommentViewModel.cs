using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Areas.ModelViews
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public string ProductName { get; set; }
        public string Name { get; set; }
        public string Context { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? customerID { get; set; }
        public int? employeeID { get; set; }
        public int? Status { get; set; }
    }
}
