using System;
using System.ComponentModel.DataAnnotations;

namespace La_Grazia.ViewModels
{
    public class LoginViewModel
    {
        //[Required]
        //[StringLength(maximumLength: 20, MinimumLength = 6)]
        //public string UserName { get; set; }

        //[Required]
        //[StringLength(maximumLength: 25, MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        [Required]
        public string Login { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}