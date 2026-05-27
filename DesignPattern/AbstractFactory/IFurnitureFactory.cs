namespace DesignPattern.AbstractFactory;

public interface IFurnitureFactory
{
    IChair CreateChair();
    ITable CreateTable();
    string StyleName { get; }
}

public class ModernFurnitureFactory : IFurnitureFactory
{
    public string StyleName => "Modern Collection";

    public IChair CreateChair() => new ModernChair();
    public ITable CreateTable() => new ModernTable();
}

public class VintageFurnitureFactory : IFurnitureFactory
{
    public string StyleName => "Vintage Collection";

    public IChair CreateChair() => new VintageChair();
    public ITable CreateTable() => new VintageTable();
}
