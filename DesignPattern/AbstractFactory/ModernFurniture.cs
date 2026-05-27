namespace DesignPattern.AbstractFactory;

public class ModernChair : IChair
{
    public string Style => "Modern";
    public string Material => "Brushed steel frame with leather cushion";
    public decimal Price => 450.00m;

    public string Describe()
    {
        return $"{Style} Chair - {Material}, ergonomic design with lumbar support (${Price})";
    }

    public string SitOn()
    {
        return "Sitting on the Modern chair: Firm support with a cool, sleek feel. Perfect for a home office.";
    }
}

public class ModernTable : ITable
{
    public string Style => "Modern";
    public string Material => "Tempered glass top with chrome legs";
    public int SeatingCapacity => 6;
    public decimal Price => 899.00m;

    public string Describe()
    {
        return $"{Style} Table - {Material}, seats {SeatingCapacity} (${Price})";
    }
}
