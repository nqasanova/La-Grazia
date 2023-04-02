using System;
using La_Grazia.Database.Models;
using Type = La_Grazia.Database.Models.Type;

namespace La_Grazia.ViewModels
{
    public class ProductViewModel
    {
        public List<Type> Types { get; set; }
        public List<Region> Regions { get; set; }
        public List<Product> Products { get; set; }
        public List<Variety> Varieties { get; set; }
    }
}