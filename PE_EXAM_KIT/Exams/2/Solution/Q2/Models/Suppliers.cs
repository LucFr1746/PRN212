using System;
using System.Collections.Generic;

namespace Q2.Models;

public partial class Suppliers
{
    public int SupplierId { get; set; }

    public string SupplierName { get; set; } = null!;

    public string ContactEmail { get; set; } = null!;

    public virtual ICollection<Products> Product { get; set; } = new List<Products>();
}
