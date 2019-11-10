using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class Department
    {
        public Department()
        {
            Division = new HashSet<Division>();
            Permission = new HashSet<Permission>();
        }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Information { get; set; }

        public virtual ICollection<Division> Division { get; set; }
        public virtual ICollection<Permission> Permission { get; set; }
    }
}
