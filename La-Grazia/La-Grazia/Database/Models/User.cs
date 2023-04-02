using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Bcpg;

namespace La_Grazia.Database.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }

        public bool IsAdmin { get; set; }

        public List<Review> Reviews { get; set; }
    }
}