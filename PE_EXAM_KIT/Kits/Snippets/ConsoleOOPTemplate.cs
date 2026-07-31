using System;
using System.Collections.Generic;
using System.Linq;

namespace Q1
{
    // ==========================================
    // 1. DELEGATE DEFINITIONS
    // ==========================================
    public delegate void ScoreUpdateHandler(string studentName, double newScore);
    public delegate void DiscountAppliedCallback(string productName, double discountPercent);

    // ==========================================
    // 2. INTERFACE DEFINITIONS
    // ==========================================
    public interface IEvaluatable
    {
        double AverageScore { get; }
        string GetRank();
    }

    public interface IDiscountable
    {
        double Price { get; set; }
        void ApplyDiscount(double percent);
    }

    // ==========================================
    // 3A. ABSTRACT CLASS — Student (Exam 1, 3)
    // ==========================================
    public abstract class Student
    {
        public string StudentId { get; set; }
        public string FullName { get; set; }
        public List<double> Scores { get; set; }

        protected Student(string studentId, string fullName)
        {
            StudentId = studentId;
            FullName = fullName;
            Scores = new List<double>();
        }

        public void AddScore(double score)
        {
            if (score < 0 || score > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 10");
            }
            Scores.Add(score);
        }

        public abstract string GetRank();
    }

    // ==========================================
    // 3B. ABSTRACT CLASS — Product (Exam 2, 4)
    // ==========================================
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

    // ==========================================
    // 4A. CONCRETE SUBCLASS — UndergraduateStudent (Exam 1, 3)
    // ==========================================
    public class UndergraduateStudent : Student, IEvaluatable
    {
        public string Major { get; set; }

        public UndergraduateStudent(string studentId, string fullName, string major)
            : base(studentId, fullName)
        {
            Major = major;
        }

        public double AverageScore
        {
            get
            {
                if (Scores.Count == 0) return 0;
                return Scores.Average();
            }
        }

        public override string GetRank()
        {
            var avg = AverageScore;
            if (avg >= 8.5) return "Excellent";
            if (avg >= 7.0) return "Good";
            if (avg >= 5.0) return "Average";
            return "Fail";
        }
    }

    // ==========================================
    // 4B. CONCRETE SUBCLASS — ElectronicsProduct (Exam 2, 4)
    // ==========================================
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

    // ==========================================
    // 5A. MANAGER CLASS — ScoreManager (Exam 1, 3)
    // ==========================================
    public class ScoreManager
    {
        private List<UndergraduateStudent> _students;
        private ScoreUpdateHandler _onScoreUpdated;

        public ScoreManager(ScoreUpdateHandler handler)
        {
            _students = new List<UndergraduateStudent>();
            _onScoreUpdated = handler;
        }

        public void AddStudent(UndergraduateStudent student)
        {
            _students.Add(student);
        }

        public void AddScoreToStudent(string studentId, double score)
        {
            var student = _students.FirstOrDefault(s => s.StudentId == studentId);
            if (student == null)
            {
                throw new InvalidOperationException($"Student not found: {studentId}");
            }

            student.AddScore(score);
            _onScoreUpdated?.Invoke(student.FullName, score);
        }

        public List<UndergraduateStudent> GetTopStudents(int n)
        {
            return _students.OrderByDescending(s => s.AverageScore).Take(n).ToList();
        }
    }

    // ==========================================
    // 5B. MANAGER CLASS — DiscountManager (Exam 2, 4)
    // ==========================================
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

    // ==========================================
    // 6. GENERIC DATA VAULT COLLECTION (Exam 5 Q1)
    // ==========================================
    public class DataVault<T>
    {
        public List<T> Items { get; set; } = new List<T>();

        public void Add(T item)
        {
            Items.Add(item);
        }

        public bool Remove(T item)
        {
            return Items.Remove(item);
        }

        public IEnumerable<T> FindAll(Func<T, bool> predicate)
        {
            return Items.Where(predicate);
        }

        public int Count()
        {
            return Items.Count;
        }
    }

    // ==========================================
    // 7A. MAIN — Student/Score variant (Exam 1, 3)
    // ==========================================
    /*
    class Program
    {
        static void Main(string[] args)
        {
            ScoreUpdateHandler handler = (name, score) =>
                Console.WriteLine($"[SCORE UPDATE]: {name} received score: {score}");

            var manager = new ScoreManager(handler);

            var s1 = new UndergraduateStudent("S1", "Nguyen Van A", "IT");
            var s2 = new UndergraduateStudent("S2", "Tran Thi B", "BA");
            var s3 = new UndergraduateStudent("S3", "Le Van C", "CS");

            manager.AddStudent(s1);
            manager.AddStudent(s2);
            manager.AddStudent(s3);

            manager.AddScoreToStudent("S1", 8.5);
            manager.AddScoreToStudent("S1", 9.0);
            manager.AddScoreToStudent("S2", 7.0);
            manager.AddScoreToStudent("S2", 7.5);
            manager.AddScoreToStudent("S3", 6.0);
            manager.AddScoreToStudent("S3", 4.5);

            try
            {
                manager.AddScoreToStudent("S1", 11.0); // Triggers ArgumentOutOfRangeException
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\n=== Top 2 Students ===");
            var top = manager.GetTopStudents(2);
            for (int i = 0; i < top.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {top[i].FullName} | Major: {top[i].Major} | Average: {top[i].AverageScore:F2} | Rank: {top[i].GetRank()}");
            }
        }
    }
    */

    // ==========================================
    // 7B. MAIN — Product/Discount variant (Exam 2, 4)
    // ==========================================
    /*
    class Program
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
    */
}
