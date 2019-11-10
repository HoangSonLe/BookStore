using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class Permission
    {
        public int PermissionId { get; set; }
        public int? DepartmentId { get; set; }
        public int? WebPageId { get; set; }
        public bool AddPermission { get; set; }
        public bool UpdatePermission { get; set; }
        public bool DeletePermission { get; set; }
        public bool SeePermission { get; set; }

        public virtual Department Department { get; set; }
        public virtual WebPage WebPage { get; set; }
    }
}
