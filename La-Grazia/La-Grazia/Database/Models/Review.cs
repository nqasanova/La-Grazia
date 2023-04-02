using System;
using System.ComponentModel.DataAnnotations;

namespace La_Grazia.Database.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rate { get; set; }

        [Required]
        [StringLength(maximumLength: 150)]
        public string Context { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsAccepted { get; set; } = false;
    }
}