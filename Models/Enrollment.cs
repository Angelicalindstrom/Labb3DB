using System;
using System.Collections.Generic;

namespace Labb3DB.Models;

public partial class Enrollment
{
    public int EnrollmentId { get; set; }

    public int FkstudentId { get; set; }

    public int FksubjectId { get; set; }

    public int FkemployeeId { get; set; }

    public string Grade { get; set; } = null!;

    public DateTime Date { get; set; }

    public virtual Employee Fkemployee { get; set; } = null!;

    public virtual Student Fkstudent { get; set; } = null!;

    public virtual Subject Fksubject { get; set; } = null!;
}
