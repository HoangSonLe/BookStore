using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class Menu
    {
        public Menu()
        {
            InverseParent = new HashSet<Menu>();
        }

        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuContent { get; set; }
        public int? ParentId { get; set; }
        public int? Status { get; set; }

        public virtual Menu Parent { get; set; }
        public virtual ICollection<Menu> InverseParent { get; set; }
    }
}
