using System;
using La_Grazia.Database.Models;

namespace La_Grazia.ViewModels
{
    public class HomeViewModel
    {
        public Setting Settings { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Slider> Sliders { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Product> FeaturedProducts { get; set; }
        public List<Product> PopularProducts { get; set; }
        public List<Collaboration> Collaborations { get; set; }
    }
}