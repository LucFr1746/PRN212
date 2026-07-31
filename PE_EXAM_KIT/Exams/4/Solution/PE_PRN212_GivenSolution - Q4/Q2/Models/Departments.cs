using System;
using System.Collections.Generic;

namespace Q2.Models;

public partial class Departments
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<Employees> Employees { get; set; } = new List<Employees>();
}
