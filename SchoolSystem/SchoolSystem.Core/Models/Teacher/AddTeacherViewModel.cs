using System.ComponentModel.DataAnnotations;
using static SchoolSystem.Infrastructure.Data.Constants.DataConstants;

namespace SchoolSystem.Core.Models.Teacher
{
    public class AddTeacherViewModel
    {
        public string UserName { get; set; } = null!;

        public int GroupId { get; set; }

        public IEnumerable<Infrastructure.Data.Entities.Group>? Groups { get; set; }

        [Range(typeof(decimal), TeacherMinSalary, TeacherMaxSalary)]
        public decimal Salary { get; set; }

        public int SubjectId { get; set; }

        public IEnumerable<Infrastructure.Data.Entities.Subject>? subjects;
    }
}
