using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace La_Grazia.Database.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public int Order { get; set; }

        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}