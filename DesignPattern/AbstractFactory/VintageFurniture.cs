namespace DesignPattern.AbstractFactory;

public class VintageChair : IChair
{
    public string Style => "Vintage";
    public string Material => "Carved oak wood with velvet upholstery";
    public decimal Price => 320.00m;

    public string Describe()
    {
        return $"{Style} Chair - {Material}, hand-crafted with floral patterns (${Price})";
    }

    public string SitOn()
    {
        return "Sitting on the Vintage chair: Soft velvet cushion with a classic, regal feel. Like sitting in a palace.";
    }
}

public class VintageTable : ITable
{
    public string Style => "Vintage";
    public string Material => "Solid mahogany with hand-carved details";
    public int SeatingCapacity => 8;
    public decimal Price => 1250.00m;

    public string Describe()
    {
        return $"{Style} Table - {Material}, seats {SeatingCapacity} (${Price})";
    }
}
