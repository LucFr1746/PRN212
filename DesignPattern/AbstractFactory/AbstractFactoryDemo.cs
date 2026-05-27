namespace DesignPattern.AbstractFactory;

public static class AbstractFactoryDemo
{
    public static void Run()
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("  ABSTRACT FACTORY PATTERN - Furniture Store  ");
        Console.WriteLine("==============================================");
        Console.WriteLine();

        Console.WriteLine("-- Step 1: Furnishing a room in Modern style --");
        Console.WriteLine();
        FurnishRoom(new ModernFurnitureFactory());

        Console.WriteLine("-- Step 2: Furnishing a room in Vintage style --");
        Console.WriteLine();
        FurnishRoom(new VintageFurnitureFactory());

        Console.WriteLine("-- Step 3: Comparing both collections side by side --");
        Console.WriteLine();

        IFurnitureFactory[] allFactories =
        [
            new ModernFurnitureFactory(),
            new VintageFurnitureFactory()
        ];

        Console.WriteLine("   +-------------+---------------+---------------+-----------+");
        Console.WriteLine("   | Style       | Chair Price   | Table Price   | Total     |");
        Console.WriteLine("   +-------------+---------------+---------------+-----------+");

        foreach (var factory in allFactories)
        {
            var chair = factory.CreateChair();
            var table = factory.CreateTable();
            var total = chair.Price + table.Price;
            Console.WriteLine($"   | {chair.Style,-11} | ${chair.Price,-12} | ${table.Price,-12} | ${total,-8} |");
        }

        Console.WriteLine("   +-------------+---------------+---------------+-----------+");
        Console.WriteLine();

        Console.WriteLine("-- Key Takeaway --");
        Console.WriteLine();
        Console.WriteLine("   The Abstract Factory ensures that products from the same family");
        Console.WriteLine("   are always used together. You can't accidentally pair a Modern");
        Console.WriteLine("   chair with a Vintage table. Swapping the entire style is as");
        Console.WriteLine("   simple as passing a different factory.");
        Console.WriteLine();
    }

    private static void FurnishRoom(IFurnitureFactory factory)
    {
        Console.WriteLine($"   Using factory: {factory.StyleName}");
        Console.WriteLine();

        IChair chair = factory.CreateChair();
        ITable table = factory.CreateTable();

        Console.WriteLine($"   {chair.Describe()}");
        Console.WriteLine($"   {table.Describe()}");
        Console.WriteLine($"   {chair.SitOn()}");
        Console.WriteLine($"   Room total: ${chair.Price + table.Price}");
        Console.WriteLine();
    }
}
