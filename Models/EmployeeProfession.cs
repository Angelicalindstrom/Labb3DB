using System;
using System.Collections.Generic;

namespace Labb3DB.Models;

public partial class EmployeeProfession
{
    public int FkemployeeId { get; set; }

    public int FkprofessionTitleId { get; set; }

    public virtual Employee Fkemployee { get; set; } = null!;

    public virtual Profession FkprofessionTitle { get; set; } = null!;
}
