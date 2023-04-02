using System;
using System.ComponentModel.DataAnnotations;

namespace La_Grazia.Database.Models
{
    public class Faq
    {
        public int Id { get; set; }
        public int Order { get; set; }

        [StringLength(maximumLength: 300)]
        public string Question { get; set; }

        [StringLength(maximumLength: 300)]
        public string Answer { get; set; }
    }
}