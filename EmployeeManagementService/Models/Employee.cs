using System;
using System.Collections.Generic;

namespace EmployeeManagementService.Models
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? DateBirth { get; set; }
        public string Post { get; set; }
        public decimal? Salary { get; set; }
        public decimal? Cash { get; set; }
    }
}
