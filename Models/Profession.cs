using System;
using System.Collections.Generic;

namespace Labb3DB.Models;

public partial class Profession
{
    public int ProfessionTitleId { get; set; }

    public string ProfessionName { get; set; } = null!;
}
