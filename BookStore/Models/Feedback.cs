using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContextSubject { get; set; }
        public string ContextMessage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Phone { get; set; }
        public bool Reply { get; set; }
        public int? EmployeeId { get; set; }
        public string ReplyContext { get; set; }
        public DateTime? ReplyDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
