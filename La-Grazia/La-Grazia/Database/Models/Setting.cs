using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace La_Grazia.Database.Models
{
    public class Setting
    {
        public int Id { get; set; }

        [StringLength(maximumLength: 100)]
        public string HeaderLogo { get; set; }

        [StringLength(maximumLength: 100)]
        public string InstagramIcon { get; set; }
        [StringLength(maximumLength: 100)]
        public string InstagramURL { get; set; }

        [StringLength(maximumLength: 100)]
        public string FacebookIcon { get; set; }
        [StringLength(maximumLength: 100)]
        public string FacebookURL { get; set; }

        [StringLength(maximumLength: 100)]
        public string TwitterIcon { get; set; }
        [StringLength(maximumLength: 100)]
        public string TwitterURL { get; set; }

        [StringLength(maximumLength: 100)]
        public string PinterestIcon { get; set; }
        [StringLength(maximumLength: 100)]
        public string PinterestURL { get; set; }
    }
}