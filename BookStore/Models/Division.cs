using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class Division
    {
        public int DivisionId { get; set; }
        public int? EmployeeId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? Division1 { get; set; }
        public bool? Validity { get; set; }

        public virtual Department Department { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
