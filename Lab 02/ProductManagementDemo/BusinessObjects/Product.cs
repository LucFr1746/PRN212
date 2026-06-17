using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ProductID")]
        public int ProductId { get; set; }

        [Column("ProductName")]
        [StringLength(40)]
        public string ProductName { get; set; } = null!;

        [Column("CategoryID")]
        public int? CategoryId { get; set; }

        [Column("UnitsInStock")]
        public short? UnitsInStock { get; set; }

        [Column("UnitPrice", TypeName = "money")]
        public decimal? UnitPrice { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
    }
}
