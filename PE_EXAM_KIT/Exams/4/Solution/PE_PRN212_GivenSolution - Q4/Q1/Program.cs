namespace Q1
{
    public delegate void DiscountAppliedCallback(string productName, double discountPercent);

    public interface IDiscountable
    {
        double Price { get; set; }
        void ApplyDiscount(double percent);
    }

    public abstract class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        protected Product(int productId, string name, double price)
        {
            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than 0");
            }
            ProductId = productId;
            Name = name;
            Price = price;
        }

        public void ApplyDiscount(double percent)
        {
            if (percent <= 0 || percent > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(percent), "Discount percent must be between 0 and 100");
            }
            Price = Price * (1 - percent / 100);
        }

        public abstract string GetCategory();
    }

    public class ElectronicsProduct : Product, IDiscountable
    {
        public string Brand { get; set; }
        public int WarrantyMonths { get; set; }

        public ElectronicsProduct(int productId, string name, double price, string brand, int warrantyMonths)
            : base(productId, name, price)
        {
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }

        public override string GetCategory()
        {
            return "Electronics";
        }

        // IDiscountable is satisfied implicitly:
        // - Price { get; set; } inherited from Product
        // - ApplyDiscount(double) inherited from Product
    }

    public class DiscountManager
    {
        private List<ElectronicsProduct> _products;
        private DiscountAppliedCallback _onDiscountApplied;

        public DiscountManager(DiscountAppliedCallback callback)
        {
            _products = new List<ElectronicsProduct>();
            _onDiscountApplied = callback;
        }

        public void AddProduct(ElectronicsProduct product)
        {
            _products.Add(product);
        }

        public void ApplyDiscountToProduct(int productId, double percent)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product not found: {productId}");
            }

            product.ApplyDiscount(percent);
            _onDiscountApplied?.Invoke(product.Name, percent);
        }

        public List<ElectronicsProduct> GetProductsUnder(double maxPrice)
        {
            return _products.Where(p => p.Price < maxPrice).OrderBy(p => p.Price).ToList();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            DiscountAppliedCallback callback = (name, percent) =>
                Console.WriteLine($"[DISCOUNT]: {name} received {percent:F1}% discount");

            var manager = new DiscountManager(callback);

            var p1 = new ElectronicsProduct(1, "Laptop Pro", 999.99, "Dell", 24);
            var p2 = new ElectronicsProduct(2, "Wireless Headphones", 150.00, "Sony", 12);
            var p3 = new ElectronicsProduct(3, "Smart Watch", 299.99, "Garmin", 12);

            manager.AddProduct(p1);
            manager.AddProduct(p2);
            manager.AddProduct(p3);

            manager.ApplyDiscountToProduct(1, 10.0);
            manager.ApplyDiscountToProduct(2, 15.0);
            manager.ApplyDiscountToProduct(3, 20.0);

            try
            {
                manager.ApplyDiscountToProduct(1, 150.0); // Triggers ArgumentOutOfRangeException
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\n=== Products Under 500.0 ===");
            var cheap = manager.GetProductsUnder(500.0);
            for (int i = 0; i < cheap.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {cheap[i].Name} | Brand: {cheap[i].Brand} | Price: {cheap[i].Price:F2} | Category: {cheap[i].GetCategory()}");
            }
        }
    }
}