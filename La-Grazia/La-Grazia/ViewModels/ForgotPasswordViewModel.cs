using System;
using System.ComponentModel.DataAnnotations;

namespace La_Grazia.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Email { get; set; }
    }
}