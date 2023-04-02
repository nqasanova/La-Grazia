using System;

namespace La_Grazia.ViewModels
{
	public class BasketProductViewModel
	{
        public int Count { get; set; }
        public int ProductId { get; set; }

        public double Price { get; set; }

        public string Name { get; set; }
        public string Image { get; set; }
    }
}