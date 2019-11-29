using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ModelViews
{
    public class CommentView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Context { get; set; }
        public string Image { get; set; }
        public int? CustomerID { get; set; }
        public int? EmployeeID { get; set; }
        public string TimeAgo { get; set; }
    }
}
