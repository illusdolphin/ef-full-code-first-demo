using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCodeFirst.Model
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Column("BirthDay", Order = 4, TypeName = "date")]
        public DateTime BirthDate { get; set; }

        [InverseProperty(nameof(Order.CreatedByPerson))]
        public virtual ICollection<Order> CreatedOrders { get; set; }

        [InverseProperty(nameof(Order.ProcessedByPerson))]
        public virtual ICollection<Order> ProcessedOrders { get; set; }
    }

}
