using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Areas.ModelViews
{
    public class OrderViewModel
    {
       public string StartDate { get; set; }
       public string EndDate { get; set; }
       public string TypeChart { get; set; }
       public List<OrderItemViewModel> orderItems { get; set; }
    }
    public class OrderItemViewModel
    {
        public int Nam { get; set; }
        public int Thang { get; set; }
        public int Tong { get; set; }
    }
}
