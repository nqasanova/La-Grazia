using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace La_Grazia.Database.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public int Order { get; set; }

        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [StringLength(maximumLength: 200)]
        public string Name { get; set; }
        [StringLength(maximumLength: 200)]
        public string Owner { get; set; }

        [StringLength(maximumLength: 2000)]
        public string Description1 { get; set; }
        [StringLength(maximumLength: 2000)]
        public string Description2 { get; set; }
        [StringLength(maximumLength: 2000)]
        public string Description3 { get; set; }
        [StringLength(maximumLength: 2000)]
        public string Description4 { get; set; }
        [StringLength(maximumLength: 2000)]
        public string Description5 { get; set; }

        public DateTime Date { get; set; }
    }
}