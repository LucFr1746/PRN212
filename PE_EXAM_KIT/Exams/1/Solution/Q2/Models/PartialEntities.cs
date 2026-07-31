using System.ComponentModel.DataAnnotations.Schema;

namespace Q2.Models
{
    public partial class Skills
    {
        [NotMapped]
        public bool IsChecked { get; set; }
    }
}
