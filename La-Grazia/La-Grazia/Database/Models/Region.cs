using System;
using System.ComponentModel.DataAnnotations;

namespace La_Grazia.Database.Models
{
    public class Region
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}