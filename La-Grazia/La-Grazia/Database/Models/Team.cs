using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace La_Grazia.Database.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [Required]
        [StringLength(maximumLength: 150)]
        public string FullName { get; set; }

        [Required]
        [StringLength(maximumLength: 150)]
        public string Profession { get; set; }

        [StringLength(maximumLength: 150)]
        public string InstagramURL { get; set; }

        [StringLength(maximumLength: 150)]
        public string FacebookURL { get; set; }

        [StringLength(maximumLength: 150)]
        public string TwitterURL { get; set; }

        [StringLength(maximumLength: 150)]
        public string PinterestURL { get; set; }
    }
}