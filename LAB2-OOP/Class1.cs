using System;
using System.Collections.Generic;
using System.Text;

namespace LAB2_OOP
{
    public class Product
    {
        private string name;
        private double price;
        private double discount;

        public string Name { get => name; set => name = value; }
        public double Price { get => price; set => price = value; }
        public double Discount { get => discount; set => discount = value; }

        public Product(string name, double price, double discount)
        {
            this.name = name;
            this.price = price;
            this.discount = discount;
        }

        public Product(string name, double price) : this(name, price, 0)
        {
        }

        private double GetImportTax()
        {
            return price * 0.1;
        }

        public void Input()
        {
            Console.Write("Enter product name: ");
            name = Console.ReadLine();

            Console.Write("Enter product price: ");
            price = double.Parse(Console.ReadLine());

            Console.Write("Enter discount amount: ");
            discount = double.Parse(Console.ReadLine());
        }

        public void Display()
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine($"Product Name: {name}");
            Console.WriteLine($"Product Price: {price:N0} VND");
            Console.WriteLine($"Discount: {discount:N0} VND");
            Console.WriteLine($"Import Tax: {GetImportTax():N0} VND");
        }
    }
}