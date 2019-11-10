using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class WebPage
    {
        public WebPage()
        {
            Permission = new HashSet<Permission>();
        }

        public int WebPageId { get; set; }
        public string WebPageName { get; set; }
        public string WebPageUrl { get; set; }

        public virtual ICollection<Permission> Permission { get; set; }
    }
}
