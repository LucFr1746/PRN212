using System;
using System.Collections.Generic;

namespace Q2.Models;

public partial class Products
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public int CategoryId { get; set; }

    public virtual Categories Category { get; set; } = null!;

    public virtual ICollection<Suppliers> Supplier { get; set; } = new List<Suppliers>();
}
