using static SchoolSystem.Infrastructure.Data.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;


namespace SchoolSystem.Core.Models.Student
{
    public class EditStudentViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(UserFirstNameMaxLength)]
        public string UserName { get; set; } = null!;

        [Required]
        [MaxLength(UserFirstNameMaxLength)]
        [MinLength(UserFirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(UserLastNameMaxLength)]
        [MinLength(UserLastNameMinLength)]
        public string LastName { get; set; } = null!;

        public int? GroupID { get; set; }

        public IEnumerable<Infrastructure.Data.Entities.Group>? Groups { get; set; } =
            new List<Infrastructure.Data.Entities.Group>();
    }
}
