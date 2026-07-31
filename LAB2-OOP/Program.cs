namespace LAB2_OOP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== ENTER PRODUCT INFORMATION ===");

            Product pd1 = new Product("", 0);
            Product pd2 = new Product("", 0);

            Console.WriteLine("Enter the first product:");
            pd1.Input();

            Console.WriteLine("\nEnter the second product:");
            pd2.Input();

            Console.WriteLine("\n=== PRODUCT INFORMATION ENTERED ===");
            pd1.Display();
            pd2.Display();

            Console.WriteLine("\n=== CONSTRUCTOR TESTING (ADVANCED) ===");

            // Product with discount
            Product p1 = new Product("iPhone 15", 25000000, 1000000);

            // Product without discount (using 2-parameter constructor)
            Product p2 = new Product("Phone Case", 200000);

            p1.Display();
            p2.Display();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}