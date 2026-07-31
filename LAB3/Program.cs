using System.Collections;

namespace LAB3 {
    internal class Program {
        static void Main(string[] args) {
            Calculator calc = new Calculator();
            calc.Calculate(10, 5);
            Console.WriteLine("=====");
            Calculator calc2 = new Calculator();
            calc2.Calculate("Hello", "World");

            Console.WriteLine("=====");
            Console.WriteLine("");
            ArrayList productList = new ArrayList();

            productList.Add(new Product { Name = "Laptop", Cost = 1200.50, Quantity = 2 });
            productList.Add(new Product { Name = "Mouse", Cost = 25.00, Quantity = 10 });
            productList.Add(new Product { Name = "Keyboard", Cost = 45.00, Quantity = 5 });
            productList.Add(new Product { Name = "Monitor", Cost = 300.00, Quantity = 3 });
            productList.Add(new Product { Name = "Headset", Cost = 60.00, Quantity = 8 });

            Console.WriteLine("--- Items in ArrayList ---");
            foreach (Product p in productList)
            {
                Console.WriteLine(p.ToString());
            }

            Console.WriteLine("=====");
            Console.WriteLine("");

            Hashtable weekDays = new Hashtable();

            weekDays.Add(1, "Monday");
            weekDays.Add(2, "Tuesday");
            weekDays.Add(3, "Wednesday");
            weekDays.Add(4, "Thursday");
            weekDays.Add(5, "Friday");
            weekDays.Add(6, "Saturday");
            weekDays.Add(7, "Sunday");

            Console.WriteLine("--- Search 'Tuesday' ---");
            if (weekDays.ContainsValue("Tuesday")) Console.WriteLine("Found: Tuesday!");
            else Console.WriteLine("Not found Tuesday.");

            Console.WriteLine("\n--- Week days list ---");
            foreach (DictionaryEntry entry in weekDays)
            {
                Console.WriteLine($"Key: {entry.Key} | Value: {entry.Value}");
            }

            Console.WriteLine("=====");
            Console.WriteLine("");

            int x = 10, y = 20;
            Console.WriteLine($"Befỏe swap: x = {x}, y = {y}");
            Swapper.Swap(ref x, ref y);
            Console.WriteLine($"After swap: x = {x}, y = {y}");

            // String
            string s1 = "Hello", s2 = "World";
            Console.WriteLine($"\nBefore swap: s1 = {s1}, s2 = {s2}");
            Swapper.Swap(ref s1, ref s2);
            Console.WriteLine($"After swap: s1 = {s1}, s2 = {s2}");
        }
    }
}
