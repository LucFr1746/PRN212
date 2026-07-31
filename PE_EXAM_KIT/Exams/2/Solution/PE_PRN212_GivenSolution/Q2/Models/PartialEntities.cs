using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q2.Models
{
    public partial class Suppliers
    {
        [NotMapped]
        public bool IsChecked { get; set; }
    }
}
