using System;
using System.ComponentModel.DataAnnotations;

namespace La_Grazia.Areas.Admin.ViewModels
{
    public class AdminViewModel
    {
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 6, ErrorMessage = "UserName can be min 6, max 50")]
        public string UserName { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 6, ErrorMessage = "FullName can be min 6, max 50")]
        public string FullName { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 6, ErrorMessage = "Email can be min 6, max 50")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 6, ErrorMessage = "Password can be min 6, max 30")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}