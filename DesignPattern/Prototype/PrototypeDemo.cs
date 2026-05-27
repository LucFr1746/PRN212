namespace DesignPattern.Prototype;

public static class PrototypeDemo
{
    public static void Run()
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("   PROTOTYPE PATTERN - Document Template Demo ");
        Console.WriteLine("==============================================");
        Console.WriteLine();

        Console.WriteLine("-- Part 1: Creating and cloning a document template --");
        Console.WriteLine();

        var originalTemplate = new DocumentTemplate
        {
            Title = "Quarterly Business Report",
            Content = "This report provides an overview of the company's financial performance, market trends, and strategic initiatives for the quarter.",
            Author = "Analytics Team",
            Category = "Business",
            Formatting = new DocumentFormatting
            {
                FontFamily = "Calibri",
                FontSize = 11,
                HeaderColor = "#1a365d",
                LineSpacing = 1.15,
                PageSize = "A4"
            },
            Tags = ["finance", "quarterly", "report", "analytics"],
            Sections = ["Executive Summary", "Revenue Analysis", "Market Trends", "Recommendations"]
        };

        originalTemplate.PrintDetails("ORIGINAL Template");
        Console.WriteLine();

        Console.WriteLine("   Cloning template for Q2 report...");
        Console.WriteLine();

        var q2Report = originalTemplate.Clone();
        q2Report.Title = "Q2 2025 Business Report";
        q2Report.Content = "Q2 performance exceeded expectations with 12% revenue growth driven by new product launches.";
        q2Report.Tags.Add("Q2-2025");
        q2Report.Sections.Add("New Product Analysis");

        q2Report.PrintDetails("Q2 CLONE (modified)");
        Console.WriteLine();

        Console.WriteLine("-- Part 2: Verifying deep copy independence --");
        Console.WriteLine();

        q2Report.Formatting.FontFamily = "Times New Roman";
        q2Report.Formatting.FontSize = 14;

        Console.WriteLine("   After changing Q2 clone's font to 'Times New Roman 14pt':");
        Console.WriteLine($"   Original formatting -> {originalTemplate.Formatting}");
        Console.WriteLine($"   Q2 clone formatting  -> {q2Report.Formatting}");
        Console.WriteLine();

        bool formatsAreIndependent = originalTemplate.Formatting.FontFamily != q2Report.Formatting.FontFamily;
        Console.WriteLine($"   Are formats independent? -> {formatsAreIndependent}");
        Console.WriteLine();

        Console.WriteLine("   After adding tags/sections only to the clone:");
        Console.WriteLine($"   Original tags:     [{string.Join(", ", originalTemplate.Tags)}]");
        Console.WriteLine($"   Q2 clone tags:     [{string.Join(", ", q2Report.Tags)}]");
        Console.WriteLine();

        bool tagsAreIndependent = originalTemplate.Tags.Count != q2Report.Tags.Count;
        Console.WriteLine($"   Are tag lists independent? -> {tagsAreIndependent}");
        Console.WriteLine();

        Console.WriteLine("-- Part 3: Using the Template Registry --");
        Console.WriteLine();

        var registry = new TemplateRegistry();

        registry.Register("Business Report", originalTemplate);

        var memoTemplate = new DocumentTemplate
        {
            Title = "Internal Memo",
            Content = "Please be advised of the following updates...",
            Author = "HR Department",
            Category = "Internal",
            Formatting = new DocumentFormatting
            {
                FontFamily = "Arial",
                FontSize = 12,
                HeaderColor = "#2d5016",
                LineSpacing = 1.5,
                PageSize = "Letter"
            },
            Tags = ["memo", "internal", "HR"],
            Sections = ["Subject", "Background", "Action Items"]
        };
        registry.Register("Internal Memo", memoTemplate);

        var invoiceTemplate = new DocumentTemplate
        {
            Title = "Invoice",
            Content = "Invoice for services rendered...",
            Author = "Billing Department",
            Category = "Financial",
            Formatting = new DocumentFormatting
            {
                FontFamily = "Courier New",
                FontSize = 10,
                HeaderColor = "#8b0000",
                LineSpacing = 1.0,
                PageSize = "A4"
            },
            Tags = ["invoice", "billing", "financial"],
            Sections = ["Bill To", "Line Items", "Total", "Payment Terms"]
        };
        registry.Register("Invoice", invoiceTemplate);

        Console.WriteLine();
        Console.WriteLine($"   Registry now has {registry.Count} templates: [{string.Join(", ", registry.ListTemplates())}]");
        Console.WriteLine();

        Console.WriteLine("   Cloning 'Invoice' from registry for a new client...");
        Console.WriteLine();

        var clientInvoice = registry.GetClone("Invoice");
        if (clientInvoice != null)
        {
            clientInvoice.Title = "Invoice #INV-2025-0042";
            clientInvoice.Content = "Web development services for Acme Corp - May 2025";
            clientInvoice.Tags.Add("acme-corp");

            clientInvoice.PrintDetails("Client Invoice (from registry clone)");
        }

        Console.WriteLine();

        Console.WriteLine("-- Key Takeaway --");
        Console.WriteLine();
        Console.WriteLine("   Instead of building each document from scratch, we clone a");
        Console.WriteLine("   pre-configured prototype and modify only what's different.");
        Console.WriteLine("   Deep copying ensures that changes to clones never affect the");
        Console.WriteLine("   original template or other clones.");
        Console.WriteLine();
    }
}
