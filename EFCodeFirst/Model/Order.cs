using EFCodeFirst.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCodeFirst.Model
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int CreatedByPersonId { get; set; }
        [ForeignKey(nameof(CreatedByPersonId))]
        public virtual Person CreatedByPerson { get; set; }

        public int? ProcessedByPersonId { get; set; }
        [ForeignKey(nameof(ProcessedByPersonId))]
        public virtual Person ProcessedByPerson { get; set; }

        [Required]
        [MaxLength(250)]
        public string Comments { get; set; }

        public OrderStatus Status { get; set; }

        [NotMapped]
        public string SomeInternalValue { get; set; }
    }
}
