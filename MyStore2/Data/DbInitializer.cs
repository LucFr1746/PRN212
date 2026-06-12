using MyStore2.Models;

namespace MyStore2.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MyStoreContext context)
        {
            // Ensure the database is created and schema exists
            context.Database.EnsureCreated();

            // Check if there are any categories already
            if (context.Categories.Any())
            {
                return; // Database has been seeded
            }

            // Seed Categories
            var categories = new Category[]
            {
                new Category { CategoryName = "Electronics", Description = "Electronic devices, gadgets and accessories" },
                new Category { CategoryName = "Clothing", Description = "Fashion items, clothes and sportswear" },
                new Category { CategoryName = "Home Appliances", Description = "Household equipment and utilities" },
                new Category { CategoryName = "Books", Description = "Educational, fiction, and non-fiction books" }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            // Seed Products
            var products = new Product[]
            {
                new Product { ProductName = "iPhone 15 Pro", Price = 999.00m, Quantity = 15, CategoryId = categories[0].CategoryId },
                new Product { ProductName = "Gaming Laptop", Price = 1499.50m, Quantity = 8, CategoryId = categories[0].CategoryId },
                new Product { ProductName = "Mechanical Keyboard", Price = 89.99m, Quantity = 35, CategoryId = categories[0].CategoryId },
                
                new Product { ProductName = "Leather Jacket", Price = 120.00m, Quantity = 20, CategoryId = categories[1].CategoryId },
                new Product { ProductName = "Running Shoes", Price = 75.50m, Quantity = 40, CategoryId = categories[1].CategoryId },
                
                new Product { ProductName = "Microwave Oven", Price = 110.00m, Quantity = 12, CategoryId = categories[2].CategoryId },
                new Product { ProductName = "Air Fryer", Price = 95.00m, Quantity = 18, CategoryId = categories[2].CategoryId },
                
                new Product { ProductName = "C# Programming in a Nutshell", Price = 45.00m, Quantity = 50, CategoryId = categories[3].CategoryId },
                new Product { ProductName = "Clean Code", Price = 37.50m, Quantity = 30, CategoryId = categories[3].CategoryId }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
