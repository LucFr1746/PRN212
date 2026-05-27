using DesignPattern.Singleton;
using DesignPattern.Factory;
using DesignPattern.AbstractFactory;
using DesignPattern.Prototype;

namespace DesignPattern;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        PrintHeader();

        while (true)
        {
            PrintMenu();
            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    SingletonDemo.Run();
                    PauseBeforeContinue();
                    break;
                case "2":
                    Console.Clear();
                    FactoryDemo.Run();
                    PauseBeforeContinue();
                    break;
                case "3":
                    Console.Clear();
                    AbstractFactoryDemo.Run();
                    PauseBeforeContinue();
                    break;
                case "4":
                    Console.Clear();
                    PrototypeDemo.Run();
                    PauseBeforeContinue();
                    break;
                case "5":
                    Console.Clear();
                    RunAll();
                    PauseBeforeContinue();
                    break;
                case "0":
                    Console.WriteLine();
                    Console.WriteLine("   Goodbye! Happy learning!");
                    Console.WriteLine();
                    return;
                default:
                    Console.WriteLine("   Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void PrintHeader()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("  ========================================================");
        Console.WriteLine("                                                            ");
        Console.WriteLine("         DESIGN PATTERNS - Learning Repository              ");
        Console.WriteLine("            Creational Patterns in C# / .NET                ");
        Console.WriteLine("                                                            ");
        Console.WriteLine("  ========================================================");
        Console.WriteLine();
    }

    private static void PrintMenu()
    {
        Console.WriteLine("  +-------------------------------------------+");
        Console.WriteLine("  | Select a pattern to explore:              |");
        Console.WriteLine("  |                                           |");
        Console.WriteLine("  |   1. Singleton Pattern                    |");
        Console.WriteLine("  |   2. Factory Method Pattern               |");
        Console.WriteLine("  |   3. Abstract Factory Pattern             |");
        Console.WriteLine("  |   4. Prototype Pattern                    |");
        Console.WriteLine("  |   5. Run ALL Demos                       |");
        Console.WriteLine("  |   0. Exit                                |");
        Console.WriteLine("  |                                           |");
        Console.WriteLine("  +-------------------------------------------+");
        Console.WriteLine();
        Console.Write("  Your choice: ");
    }

    private static void RunAll()
    {
        Console.WriteLine("  Running ALL pattern demos...");
        Console.WriteLine("  ========================================================");
        Console.WriteLine();

        SingletonDemo.Run();
        PrintSeparator();

        FactoryDemo.Run();
        PrintSeparator();

        AbstractFactoryDemo.Run();
        PrintSeparator();

        PrototypeDemo.Run();

        Console.WriteLine("  ========================================================");
        Console.WriteLine("  All demos completed!");
        Console.WriteLine();
    }

    private static void PrintSeparator()
    {
        Console.WriteLine();
        Console.WriteLine("  --------------------------------------------------------");
        Console.WriteLine();
    }

    private static void PauseBeforeContinue()
    {
        Console.WriteLine();
        Console.Write("  Press any key to return to the menu...");
        Console.ReadKey(intercept: true);
        Console.Clear();
        PrintHeader();
    }
}
