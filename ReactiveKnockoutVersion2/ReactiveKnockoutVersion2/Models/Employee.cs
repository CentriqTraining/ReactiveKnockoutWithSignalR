using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReactiveKnockoutVersion2.Models
{
    public class Employee
    {
        public Employee()
        {
            IsLocked = false;
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public DateTime? TerminationDate { get; set; }
        public bool IsLocked { get; set; }
        public string LockHolder { get; set; }

    }

}