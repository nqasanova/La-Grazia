using System;
using System.ComponentModel.DataAnnotations;
using La_Grazia.Database.Models.Enums;

namespace La_Grazia.Database.Models
{
    public class OrderedProduct
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int? ProductId { get; set; }
        public Product Product { get; set; }

        public double SalePrice { get; set; }
        public double Price { get; set; }

        [StringLength(maximumLength: 100)]
        public string ProductName { get; set; }

        [StringLength(maximumLength: 100)]
        public string ProductImage { get; set; }
    }
}