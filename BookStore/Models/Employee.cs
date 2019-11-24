using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Comment = new HashSet<Comment>();
            Feedback = new HashSet<Feedback>();
            InverseManager = new HashSet<Employee>();
            Orders = new HashSet<Orders>();
        }

        public int EmployeeId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityCardNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool? Sex { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Role { get; set; }
        public int? ManagerId { get; set; }
        public string Image { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Employee Manager { get; set; }
        public virtual Roles RoleNavigation { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Feedback> Feedback { get; set; }
        public virtual ICollection<Employee> InverseManager { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
