﻿//  Employee Change Data Transfer object
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Reactive.Models
{
    public class EmployeeChange_DTO
    {
        public int Id { get; set; }
        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }
    }
}