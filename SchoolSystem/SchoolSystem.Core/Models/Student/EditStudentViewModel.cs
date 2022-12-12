using static SchoolSystem.Data.Data.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;


namespace SchoolSystem.Core.Models.Student
{
    public class EditStudentViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(UserFirstNameMaxLength)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(UserFirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(UserLastNameMaxLength)]
        public string LastName { get; set; }

        public int? GroupID { get; set; }

        public IEnumerable<SchoolSystem.Data.Data.Entities.Group> Groups { get; set; } =
            new List<SchoolSystem.Data.Data.Entities.Group>();
    }
}
