namespace DesignPattern.Prototype;

public interface IPrototype<T> where T : class
{
    T Clone();
}
