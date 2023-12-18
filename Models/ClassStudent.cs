using System;
using System.Collections.Generic;

namespace Labb3DB.Models;

public partial class ClassStudent
{
    public int FkclassId { get; set; }

    public int FkstudentId { get; set; }

    public virtual Class Fkclass { get; set; } = null!;

    public virtual Student Fkstudent { get; set; } = null!;
}
