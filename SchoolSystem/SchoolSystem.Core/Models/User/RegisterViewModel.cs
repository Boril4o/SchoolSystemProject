using static SchoolSystem.Infrastructure.Data.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;


namespace SchoolSystem.Core.Models.User
{
    public class RegisterViewModel
    {
        [Required]
        [MaxLength(UserFirstNameMaxLength)]
        [MinLength(UserFirstNameMinLength)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(UserFirstNameMaxLength)]
        [MinLength(UserFirstNameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(UserLastNameMaxLength)]
        [MinLength(UserLastNameMinLength)]
        public string LastName { get; set; }

        [Range(UserMinAge, UserMaxAge)]
        public int Age { get; set; }

        [Required]
        [MaxLength(UserEmailMaxLength)]
        [MinLength(UserEmailMinLength)]
        public string Email { get; set; }

        [Required]
        [MaxLength(UserPasswordMaxLength)]
        [MinLength(UserPasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
