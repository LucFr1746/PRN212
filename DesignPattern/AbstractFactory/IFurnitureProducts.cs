namespace DesignPattern.AbstractFactory;

public interface IChair
{
    string Style { get; }
    string Material { get; }
    decimal Price { get; }
    string Describe();
    string SitOn();
}

public interface ITable
{
    string Style { get; }
    string Material { get; }
    int SeatingCapacity { get; }
    decimal Price { get; }
    string Describe();
}
