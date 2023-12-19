using System;
using System.Collections.Generic;

namespace Labb3DB.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public decimal? Salary { get; set; }

    public DateTime? EmployeeStartDate { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
