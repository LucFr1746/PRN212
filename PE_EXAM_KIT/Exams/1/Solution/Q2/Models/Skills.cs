using System;
using System.Collections.Generic;

namespace Q2.Models;

public partial class Skills
{
    public int SkillId { get; set; }

    public string SkillName { get; set; } = null!;

    public virtual ICollection<Employees> Employee { get; set; } = new List<Employees>();
}
