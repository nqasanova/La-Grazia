using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace La_Grazia.Database.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int TypeId { get; set; }
        public Type Type { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }

        public int VarietyId { get; set; }
        public Variety Variety { get; set; }

        [Required]
        [StringLength(maximumLength: 150)]
        public string Name { get; set; }

        [StringLength(maximumLength: 1000)]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public double SalePrice { get; set; }

        [Required]
        public bool IsFeatured { get; set; }

        [Required]
        public int Rate { get; set; }

        public List<Review> Reviews { get; set; }

        [NotMapped]
        public List<IFormFile> ImageFiles { get; set; }

        public List<OrderedProduct> OrderedProducts { get; set; }

        public List<ProductImage> ProductImages { get; set; }
        [NotMapped]
        public List<int> ProductImageIds { get; set; } = new List<int>();
    }
}