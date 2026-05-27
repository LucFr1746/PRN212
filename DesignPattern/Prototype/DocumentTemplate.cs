namespace DesignPattern.Prototype;

public class DocumentFormatting
{
    public string FontFamily { get; set; } = "Arial";
    public int FontSize { get; set; } = 12;
    public string HeaderColor { get; set; } = "#333333";
    public double LineSpacing { get; set; } = 1.5;
    public string PageSize { get; set; } = "A4";

    public DocumentFormatting Clone()
    {
        return (DocumentFormatting)MemberwiseClone();
    }

    public override string ToString()
    {
        return $"Font: {FontFamily} {FontSize}pt | Header: {HeaderColor} | Spacing: {LineSpacing}x | Page: {PageSize}";
    }
}

public class DocumentTemplate : IPrototype<DocumentTemplate>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DocumentFormatting Formatting { get; set; } = new();
    public List<string> Tags { get; set; } = [];
    public List<string> Sections { get; set; } = [];

    public DocumentTemplate Clone()
    {
        var clone = (DocumentTemplate)MemberwiseClone();

        clone.Formatting = Formatting.Clone();
        clone.Tags = new List<string>(Tags);
        clone.Sections = new List<string>(Sections);
        clone.CreatedDate = DateTime.Now;

        return clone;
    }

    public void PrintDetails(string label)
    {
        Console.WriteLine($"   --- {label} ---");
        Console.WriteLine($"   Title:    {Title}");
        Console.WriteLine($"   Author:   {Author}");
        Console.WriteLine($"   Category: {Category}");
        Console.WriteLine($"   Created:  {CreatedDate:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine($"   Format:   {Formatting}");
        Console.WriteLine($"   Tags:     [{string.Join(", ", Tags)}]");
        Console.WriteLine($"   Sections: [{string.Join(", ", Sections)}]");
        Console.WriteLine($"   Content:  {(Content.Length > 60 ? Content[..60] + "..." : Content)}");
        Console.WriteLine($"   ---");
    }
}
