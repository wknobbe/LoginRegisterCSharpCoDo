using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginRegisterCSharpCoDo.Models
{
    public class User
    {
        [Key]
        [Required]
        public int UserId {get;set;}
        [Required]
        [MinLength(2, ErrorMessage="First name must be at least 2 characters long.")]
        public string FirstName {get;set;}
        [Required]
        [MinLength(2, ErrorMessage="Last name must be at least 2 characters long.")]
        public string LastName {get;set;}
        [Required]
        [EmailAddress]
        public string Email {get;set;}
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password must contain at least 8 characters.")]
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword {get;set;}
    }
}