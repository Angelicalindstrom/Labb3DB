using System;
using System.Collections.Generic;

namespace Labb3DB.Models;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string SubjectName { get; set; } = null!;

    public int FkemployeeId { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Employee Fkemployee { get; set; } = null!;
}
