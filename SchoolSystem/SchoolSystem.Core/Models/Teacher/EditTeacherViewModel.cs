using static SchoolSystem.Infrastructure.Data.Constants.DataConstants;
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

        public int GroupId { get; set; }

        public IEnumerable<Infrastructure.Data.Entities.Group>? Groups =
            new List<Infrastructure.Data.Entities.Group>();

        public int SubjectId { get; set; }

        public IEnumerable<Infrastructure.Data.Entities.Subject>? Subjects =
            new List<Infrastructure.Data.Entities.Subject>();

        [Range(typeof(decimal), TeacherMinSalary, TeacherMaxSalary)]
        public decimal Salary { get; set; }

        public int Id { get; set; }
    }
}
