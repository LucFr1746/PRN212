using System;
using System.Collections.Generic;

namespace Q2.Models;

public partial class Employees
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public decimal Salary { get; set; }

    public DateOnly HireDate { get; set; }

    public int DepartmentId { get; set; }

    public virtual Departments Department { get; set; } = null!;

    public virtual ICollection<Skills> Skill { get; set; } = new List<Skills>();
}
