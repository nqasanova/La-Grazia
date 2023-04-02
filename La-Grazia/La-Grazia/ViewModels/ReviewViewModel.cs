using System;
using La_Grazia.Database.Models;

namespace La_Grazia.ViewModels
{
    public class ReviewViewModel
    {
        public int Rate { get; set; }
        public string Context { get; set; }

        public Product Product { get; set; }
        public List<Review> Reviews { get; set; }
    }
}