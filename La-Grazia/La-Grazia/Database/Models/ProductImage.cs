using System;
namespace La_Grazia.Database.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string Image { get; set; }
    }
}