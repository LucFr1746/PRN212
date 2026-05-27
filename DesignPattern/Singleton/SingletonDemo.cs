namespace DesignPattern.Singleton;

public static class SingletonDemo
{
    public static void Run()
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("       SINGLETON PATTERN - Logger Demo        ");
        Console.WriteLine("==============================================");
        Console.WriteLine();

        Console.WriteLine("-- Step 1: Getting logger instances from different modules --");
        Console.WriteLine();

        var loggerFromAuth = AppLogger.Instance;
        var loggerFromPayment = AppLogger.Instance;
        var loggerFromInventory = AppLogger.Instance;

        Console.WriteLine("-- Step 2: Verifying all references point to the same object --");
        Console.WriteLine();

        bool authEqualsPayment = ReferenceEquals(loggerFromAuth, loggerFromPayment);
        bool paymentEqualsInventory = ReferenceEquals(loggerFromPayment, loggerFromInventory);

        Console.WriteLine($"   loggerFromAuth == loggerFromPayment?     -> {authEqualsPayment}");
        Console.WriteLine($"   loggerFromPayment == loggerFromInventory? -> {paymentEqualsInventory}");
        Console.WriteLine();

        Console.WriteLine("-- Step 3: Logging from different modules --");
        Console.WriteLine();

        loggerFromAuth.Info("User 'john_doe' logged in successfully");
        loggerFromAuth.Info("JWT token generated for user 'john_doe'");

        loggerFromPayment.Warning("Payment gateway response delayed (2.3s)");
        loggerFromPayment.Info("Payment of $49.99 processed for order #1234");

        loggerFromInventory.Error("Stock level critical for SKU-7891 (2 remaining)");
        loggerFromInventory.Info("Restock order created for SKU-7891");

        Console.WriteLine();

        Console.WriteLine("-- Step 4: All logs collected in a single instance --");
        Console.WriteLine();
        Console.WriteLine($"   Total logs across entire application: {loggerFromAuth.TotalLogCount}");
        Console.WriteLine($"   (Same count from any reference: {loggerFromInventory.TotalLogCount})");
        Console.WriteLine();

        Console.WriteLine("-- Key Takeaway --");
        Console.WriteLine();
        Console.WriteLine("   No matter how many times you call AppLogger.Instance,");
        Console.WriteLine("   you always get the SAME object. All log entries from");
        Console.WriteLine("   all modules end up in one centralized place.");
        Console.WriteLine();
    }
}
