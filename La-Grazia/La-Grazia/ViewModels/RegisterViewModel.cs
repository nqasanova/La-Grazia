using System;
using System.ComponentModel.DataAnnotations;

namespace La_Grazia.ViewModels
{
    public class RegisterViewModel
    {
        //[StringLength(maximumLength: 20, MinimumLength = 6)]
        //public string UserName { get; set; }

        //[StringLength(maximumLength: 50, MinimumLength = 6)]
        //public string FullName { get; set; }

        //[StringLength(maximumLength: 100, MinimumLength = 6)]
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }

        //[StringLength(maximumLength: 25, MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        //[StringLength(maximumLength: 25, MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Compare(nameof(Password))]
        //public string ConfirmPassword { get; set; }

        [Required]
        public string Fullname { get; set; }
        [Required, MaxLength(30)]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Range(10, 100)]
        public byte Age { get; set; }

        [Required]
        public string Position { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}