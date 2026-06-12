namespace MyStore2.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation property for related Products
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
