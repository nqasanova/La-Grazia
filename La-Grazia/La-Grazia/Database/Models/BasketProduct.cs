using System;

namespace La_Grazia.Database.Models
{
    public class BasketProduct 
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}