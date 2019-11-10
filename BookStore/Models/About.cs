using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class About
    {
        public int AboutId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AboutImage { get; set; }
        public int? Status { get; set; }
    }
}
