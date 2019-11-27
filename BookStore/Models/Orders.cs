using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class Orders
    {
        public Orders()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PayMethod { get; set; }
        public string ShipMethod { get; set; }
        public double? ShipCost { get; set; }
        public string Comment { get; set; }
        public int? OrderStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public int? Total { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
