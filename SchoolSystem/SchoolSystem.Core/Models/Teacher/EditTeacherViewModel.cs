using static SchoolSystem.Data.Data.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;


namespace SchoolSystem.Core.Models.Teacher
{
    public class EditTeacherViewModel
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

        public int? GroupId { get; set; }

        public IEnumerable<SchoolSystem.Data.Data.Entities.Group> Groups =
            new List<SchoolSystem.Data.Data.Entities.Group>();

        public int SubjectId { get; set; }

        public IEnumerable<SchoolSystem.Data.Data.Entities.Subject> Subjects =
            new List<SchoolSystem.Data.Data.Entities.Subject>();

        [Range(typeof(decimal), TeacherMinSalary, TeacherMaxSalary)]
        public decimal Salary { get; set; }

        public int Id { get; set; }
    }
}
