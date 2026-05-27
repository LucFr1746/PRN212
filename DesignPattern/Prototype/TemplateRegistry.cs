namespace DesignPattern.Prototype;

public class TemplateRegistry
{
    private readonly Dictionary<string, DocumentTemplate> _templates = new();

    public void Register(string name, DocumentTemplate template)
    {
        _templates[name] = template.Clone();
        Console.WriteLine($"   Registered template: '{name}'");
    }

    public DocumentTemplate? GetClone(string name)
    {
        if (_templates.TryGetValue(name, out var template))
        {
            return template.Clone();
        }

        Console.WriteLine($"   Template '{name}' not found in registry");
        return null;
    }

    public IEnumerable<string> ListTemplates() => _templates.Keys;

    public int Count => _templates.Count;
}
