using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects
{
    [Table("AccountMember")]
    public class AccountMember
    {
        [Key]
        [Column("MemberID")]
        [StringLength(20)]
        public string MemberId { get; set; } = null!;

        [Column("MemberPassword")]
        [StringLength(80)]
        public string MemberPassword { get; set; } = null!;

        [Column("FullName")]
        [StringLength(80)]
        public string FullName { get; set; } = null!;

        [Column("EmailAddress")]
        [StringLength(100)]
        public string? EmailAddress { get; set; }

        [Column("MemberRole")]
        public int? MemberRole { get; set; }
    }
}
