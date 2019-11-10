using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class Contact
    {
        public int ContactId { get; set; }
        public string Content { get; set; }
        public int? Status { get; set; }
    }
}
