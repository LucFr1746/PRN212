using System;
using System.Collections.Generic;
using System.Text;

namespace LAB3
{
    internal class Product
    {
        public string Name { get; set; }
        public double Cost { get; set; }
        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"Product: {Name}, Cost: {Cost:C}, Quantity: {Quantity}";
        }
    }
}
