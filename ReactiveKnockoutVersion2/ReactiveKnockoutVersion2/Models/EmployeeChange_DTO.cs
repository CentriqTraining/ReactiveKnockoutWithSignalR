//  Employee Change Data Transfer object
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReactiveKnockoutVersion2.Models
{
    public class EmployeeChange_DTO
    {
        public int Id { get; set; }
        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }
    }
}