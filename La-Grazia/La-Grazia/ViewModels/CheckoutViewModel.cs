using System;
using System.ComponentModel.DataAnnotations;

namespace La_Grazia.ViewModels
{
    public class CheckoutViewModel
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(maximumLength: 150)]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 30)]
        public string Phone { get; set; }

        [Required]
        [StringLength(maximumLength: 300)]
        public string Address { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Country { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string City { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string ZipCode { get; set; }

        [StringLength(maximumLength: 450)]
        public string Note { get; set; }

        public List<BasketProductViewModel> BasketProductViewModels { get; set; } = new List<BasketProductViewModel>();
    }
}