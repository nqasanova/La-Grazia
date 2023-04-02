using System;
using System.ComponentModel.DataAnnotations;
using La_Grazia.Database.Models.Enums;

namespace La_Grazia.Database.Models
{
    public class Order
    {
        [Key]
        public double Total { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        [StringLength(maximumLength: 250)]
        public string Address { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string FullName { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        public string Phone { get; set; }

        [StringLength(maximumLength: 500)]
        public string Note { get; set; }

        [StringLength(maximumLength: 50)]
        public string Country { get; set; }

        [StringLength(maximumLength: 50)]
        public string City { get; set; }

        [StringLength(maximumLength: 50)]
        public string ZipCode { get; set; }

        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }

        public IEnumerable<OrderedProduct> OrderedProducts { get; set; }
    }
}