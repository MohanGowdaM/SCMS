using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartClinic.Domain.Entities
{
    [Table("TokenSequence")]
    public partial class TokenSequence
    {
        [Key]
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public int CurrentSequence { get; set; }

        [Column(TypeName = "date")]
        public DateTime LastResetDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDateTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
    }
}
