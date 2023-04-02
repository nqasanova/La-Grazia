using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace La_Grazia.Database.Models
{
    public class About
    {
        public int Id { get; set; }

        [StringLength(maximumLength: 200)]
        public string Title { get; set; }

        [StringLength(maximumLength: 1000)]
        public string Description { get; set; }

        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}